using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IconBoard : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material material;
    public Texture2D texture;
    
    private bool resetPressed;
    private bool confirmPressed;

    public SpriteRenderer resetButtonRenderer;
    public SpriteRenderer confirmButtonRenderer;
    public Sprite[] sprits;
    
    public int radius = 8;
    int offset = 4;
    public Color paintColor;

    public int minX, maxX, minY, maxY;

    private Vector2 _prevDraw = Vector2.zero;


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


    public void StartPainting()
    {
        texture = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
        init_texture2d();
        material.mainTexture = texture;
        meshRenderer.enabled = true;
        // overlayCanvas.SetActive(true);
        paintColor = paintColor * 2.0f;
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
                    // pixelArray[index] = new Color(98.0f / 255.0f, 114.0f / 255.0f, 164.0f / 255.0f);
                    pixelArray[index] = Color.white;
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
        // canvas.SetActive(false);
        // GameManager.instance.palette.gameObject.SetActive(false);
        // painting = false;

        // GameObject clonedMesh = Instantiate(meshRenderer.gameObject);
        // clonedMesh.transform.position = meshRenderer.transform.position;
        // clonedMesh.transform.position = new Vector3(clonedMesh.transform.position.x, clonedMesh.transform.position.y, 0.0f);
        // clonedMesh.layer = 6;

        /*Vector3 scale = clonedMesh.transform.localScale;
        clonedMesh.transform.localScale = scale * paintTarget.camerafraction;
        paintTarget.meshRenderer = clonedMesh.GetComponent<MeshRenderer>();
        paintTarget.meshRenderer.material = new Material(material);
        paintTarget.texture = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(material.mainTexture, paintTarget.texture);
        paintTarget.meshRenderer.material.mainTexture = paintTarget.texture;

        meshRenderer.enabled = false;*/
        // overlayCanvas.SetActive(false);
        // clonedMesh.transform.parent = paintTarget.transform;

        /*int width = renderTexture.width;
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
        
        // CreateSpriteFromTexture(pixelArray, renderTexture.width, renderTexture.height);

        GameObject gameObject = new GameObject();
        gameObject.transform.position = Camera.main.transform.position;
        SpriteRenderer sprite = gameObject.AddComponent<SpriteRenderer>();
        sprite.sprite = Sprite.Create(textureResized, new Rect(0, 0, textureResized.width, textureResized.height), new Vector2(0.5f, 0.5f));
        targetSpriteRenderer.sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));

        overlayCanvas.SetActive(false);
        Destroy(newTexture);*/
    }

    void DrawPixel(int x, int y, int size, Color color)
    {
        x = (x / offset) * offset;
        y = (y / offset) * offset;
        for (int i = -size / 2; i <= size / 2; i++)
        {
            for (int j = -size / 2; j <= size / 2; j++)
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
        Debug.Log(new Vector2Int(x, y));
    }

    void Painting()
    {
        if (Input.GetMouseButton(0))
        {
            // print mouse position
            Vector2 mousePosition = Input.mousePosition;
            float x = mousePosition.x;
            float y = mousePosition.y;
            if (!(x < minX || y < minY || x > maxX || y > maxY))
            {
                Vector2Int pixelPosition = new Vector2Int((int)mousePosition.x, (int)mousePosition.y);
                if (_prevDraw.magnitude > 0)
                {
                    Vector2Int prevPixelPosition = new Vector2Int((int)_prevDraw.x, (int)_prevDraw.y);
                    Vector2Int delta = pixelPosition - prevPixelPosition;
                    int steps = (int)delta.magnitude;
                    for (int i = 0; i < steps; i += offset)
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
        }

        if (Input.GetMouseButtonUp(0))
        {
            _prevDraw = Vector2.zero;
        }
    }

    private bool ResetButtonPressed(float x, float y)
    {
        return (x > 394 && x < 523 && y > 137 && y < 224);
    }

    private bool ConfirmButtonPressed(float x, float y)
    {
        return (x > 805 && x < 930 && y > 137 && y < 224);
    }

    private void Awake()
    {
        material = meshRenderer.material;
    }


    // Start is called before the first frame update
    void Start()
    {
        StartPainting();
        DontDestroyOnLoad(meshRenderer.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(StartPaintingIE());
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            float x = mousePosition.x;
            float y = mousePosition.y;
            if(ResetButtonPressed(x, y))
            {
                resetPressed = true;
                resetButtonRenderer.sprite = sprits[1];
            }
            if(ConfirmButtonPressed(x, y))
            {
                confirmPressed = true;
                confirmButtonRenderer.sprite = sprits[3];
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (resetPressed)
            {
                resetPressed = false;
                resetButtonRenderer.sprite = sprits[0];
                init_texture2d();
            }
            if (confirmPressed)
            {
                confirmPressed = false;
                confirmButtonRenderer.sprite = sprits[2];
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        // if (painting)
        // {
            Painting();
            texture.Apply();
        // }

    }
}

