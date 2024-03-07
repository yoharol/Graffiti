using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WhiteBoard : MonoBehaviour
{
    public GameObject canvas;
    public GameObject preset;
    public MeshRenderer meshRenderer;
    public Material material;
    public Texture2D texture;
    public GameObject overlayCanvas;

    public PolygonCollider2D boardCollider;
    public bool painting = false;
    public bool finished = false;
    public int radius = 8;
    int offset = 8;
    public Color paintColor;

    public RenderTexture renderTexture;
    public SpriteRenderer targetSpriteRenderer;
    public PaintTarget paintTarget;

    public Color[] palette;

    private Vector2 _prevDraw = Vector2.zero;
    private float start_paint_time;

    void init_texture2d()
    {
        Color transparentColor = new Color(0, 0, 0, 0);
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, transparentColor);
            }
        }
        texture.Apply();
    }

    public IEnumerator StartPaintingIE(PaintTarget target)
    {
        start_paint_time = Time.time;
        finished = false;   
        GameManager.instance.whiteBoardMode = false;
        GameManager.instance.playerController.enabled = false;
        paintTarget = target;
        // GameManager.instance.playerController.gameObject.SetActive(false);
        float cameraMoveTime = 1.0f;
        CameraManager.instance.SetCameraToFraction(target.paintLookTarget.transform.position, target.camerafraction, cameraMoveTime, false);
        
        // ===================================================
        // canvas.SetActive(true);
        yield return new WaitForSeconds(cameraMoveTime);
        // preset.SetActive(true);
        StartPainting();
    }

    public IEnumerator StartWhiteBoardIE()
    {
        start_paint_time = Time.time;
        finished = false;
        GameManager.instance.whiteBoardMode = true;
        GameManager.instance.playerController.enabled = false;
        canvas.SetActive(true);
        preset.SetActive(true);
        painting = true;
        GameManager.instance.palette.gameObject.SetActive(true);
        texture = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
        init_texture2d();
        material.mainTexture = texture;
        meshRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        StartPainting();
    }

    public void StartPainting()
    {
        painting = true;
        overlayCanvas.SetActive(true);
        GameManager.instance.palette.gameObject.SetActive(true);
        texture = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
        init_texture2d();
        material.mainTexture = texture;
        meshRenderer.enabled = true;
    }

    void FillBackground(Color[] pixelArray, int width, int height)
    {   
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(Vector2Int.zero);

        while (queue.Count > 0)
        {
            Vector2Int pos = queue.Dequeue();
            int x = pos.x;
            int y = pos.y;

            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                int index = y * width + x;

                if (pixelArray[index].a == 0)
                {
                    pixelArray[index] = Color.red;
                    queue.Enqueue(new Vector2Int(x - 1, y));
                    queue.Enqueue(new Vector2Int(x + 1, y));
                    queue.Enqueue(new Vector2Int(x, y - 1));
                    queue.Enqueue(new Vector2Int(x, y + 1));
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = y * width + x;
                if (pixelArray[index] == Color.red)
                {
                    pixelArray[index] = Color.clear;
                }
                else if (pixelArray[index].a == 0.0f)
                {
                    pixelArray[index] = palette[0];
                }
            }
        }
    }
    
    float ColorNorm(Color c)
    {
        return c.r + c.g + c.b;
    }

    public void FinishPainting()
    {
        canvas.SetActive(false);
        GameManager.instance.palette.gameObject.SetActive(false);
        painting = false;

        GameObject clonedMesh = Instantiate(meshRenderer.gameObject);
        clonedMesh.transform.position = meshRenderer.transform.position;
        clonedMesh.transform.position = new Vector3(clonedMesh.transform.position.x, clonedMesh.transform.position.y, 0.0f);
        clonedMesh.layer = 6;

        Vector3 scale = clonedMesh.transform.localScale;
        clonedMesh.transform.localScale = scale * paintTarget.camerafraction;
        paintTarget.meshRenderer = clonedMesh.GetComponent<MeshRenderer>();
        paintTarget.meshRenderer.material = new Material(material);
        paintTarget.texture = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(material.mainTexture, paintTarget.texture);
        paintTarget.meshRenderer.material.mainTexture = paintTarget.texture;

        meshRenderer.enabled = false;
    }

    public void FinishWhiteBoard()
    {
        canvas.SetActive(false);
        GameManager.instance.palette.gameObject.SetActive(false);
        painting = false;
        // clonedMesh.transform.parent = paintTarget.transform;

        int width = renderTexture.width;
        int height = renderTexture.height;

        Texture2D newTexture = new Texture2D(width, height , TextureFormat.RGBA32, false, false);
        RenderTexture.active = renderTexture;
        newTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        newTexture.Apply();
        
        Color[] pixelArray = newTexture.GetPixels();
        Color[] pixelArrayResized = new Color[(renderTexture.height / 4) * (renderTexture.width / 4)];
        for (int x = 0; x < renderTexture.width; x+=1)
        {
            for (int y = 0; y < renderTexture.height; y+=1)
            {
                int index = y * renderTexture.width + x;
                int indexResized = (y / 4) * (renderTexture.width / 4) + (x / 4);
                pixelArrayResized[indexResized] = pixelArray[index];
            }
        }
        FillBackground(pixelArray, renderTexture.width, renderTexture.height);
        FillBackground(pixelArrayResized, renderTexture.width / 4, renderTexture.height / 4);

        Texture2D textureResized = new Texture2D(renderTexture.width / 4, renderTexture.height / 4, TextureFormat.RGBA32, false);
        textureResized.SetPixels(pixelArrayResized);
        textureResized.filterMode = FilterMode.Point;
        textureResized.Apply();

        targetSpriteRenderer.sprite = Sprite.Create(textureResized, new Rect(0, 0, textureResized.width, textureResized.height), new Vector2(0.5f, 0.5f));

        overlayCanvas.SetActive(false);
        Destroy(newTexture);
    }

    void CreateSpriteFromTexture(Color[] pixelArray, int width, int height)
    {
        // create a new gameobject with sprite target
        GameObject gameObject = new GameObject();
        gameObject.transform.position = Camera.main.transform.position;
        SpriteRenderer sprite = gameObject.AddComponent<SpriteRenderer>();
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.SetPixels(pixelArray);
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        sprite.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        Destroy(texture);
    }

    void DrawPixel(int x, int y, int size, Color color)
    {
        x = (x / offset) * offset;
        y = (y / offset) * offset;
        for (int i = -size/2; i <= size/2; i++)
        {
            for (int j = -size/2; j <= size/2; j++)
            {
                if (i * i + j * j < size * size)
                {
                    int newx = x + i;
                    int newy = y + j;
                    if (newx >= 0 && newx < texture.width && newy >= 0 && newy < texture.height)
                    {
                        texture.SetPixel(newx, newy, color);
                    }
                }
            }
        }
    }

    void Painting()
    {
        UnityEngine.Debug.Log("Painting");
        if (Input.GetMouseButton(0))
        {
            // print mouse position
            Vector2 mousePosition = Input.mousePosition;
            Vector2Int pixelPosition = new Vector2Int((int)mousePosition.x, (int)mousePosition.y);
            if (_prevDraw.magnitude > 0)
            {
                Vector2Int prevPixelPosition = new Vector2Int((int)_prevDraw.x, (int)_prevDraw.y);
                Vector2Int delta = pixelPosition - prevPixelPosition;
                int steps = (int)delta.magnitude;
                for (int i = 0; i < steps; i+=offset)
                {
                    Vector2Int drawPosition = prevPixelPosition + delta * i / steps;
                    DrawPixel(drawPosition.x, drawPosition.y, radius, paintColor);
                }
            }
            else
            {
                DrawPixel(pixelPosition.x, pixelPosition.y, radius, paintColor);
            }
            _prevDraw = pixelPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _prevDraw = Vector2.zero;
        }
    }

    private void Awake()
    {
        material = meshRenderer.material;
    }


    // Start is called before the first frame update
    void Start()
    {

    }
 
    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(StartPaintingIE());
        }*/

        if (painting && Input.GetKeyDown(KeyCode.E))
        {
            if(Time.time - start_paint_time < 0.5f)
                return;
            if (GameManager.instance.whiteBoardMode)
                FinishWhiteBoard();
            else
                FinishPainting();
            finished = true;
            // GameManager.instance.playerController.gameObject.SetActive(true);
            CameraManager.instance.SetCameraToFollowPlayer(1.0f);
            GameManager.instance.playerController.enabled = true;
        }

        if (painting)
        {
            Painting();
            texture.Apply();
        }

    }
}
