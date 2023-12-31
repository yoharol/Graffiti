using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerTrajectory : MonoBehaviour
{
    public Transform playerTrans;
    public Color brushColor;
    public int offset = 20;
    public int brushSize = 2;

    public int arraySize = 5;
    public GameObject TrajPrefab;
    public Material TrajMaterial;
    private GameObject[,] TrajList;
    private Material[,] MatList;
    private Texture2D[,] TexList; 
    
    
    void init_texture2d(Texture2D texture, Color c)
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, c);
            }
        }
        texture.Apply();
    }

    void drawOnTexture()
    {
        float x = playerTrans.position.x;
        float y = playerTrans.position.y;
        int i = Mathf.RoundToInt(x / 10.0f) + arraySize / 2;
        int j = Mathf.RoundToInt(y / 10.0f) + arraySize / 2;
        if (i < 0 || i >= arraySize || j < 0 || j >= arraySize)
            return;
        int offx = (int)(((x + 5.0f) % 10.0f)/10.0f * (1000 / offset)); 
        int offy = (int)(((y + 5.0f) % 10.0f)/10.0f * (1000 / offset)); 
        for (int bx = -brushSize/2; bx < brushSize/2; bx++)
            for (int by = -brushSize/2; by < brushSize/2; by++)
                TexList[i,j].SetPixel(offx+bx, offy+by, brushColor);
        TexList[i,j].Apply();

    }
    
    // Start is called before the first frame update
    void Start()
    {
        // generate arraySize x arraySize array of TrajPrefab
        TrajList = new GameObject[arraySize, arraySize];
        MatList = new Material[arraySize,arraySize];
        TexList = new Texture2D[arraySize, arraySize];

        for (int i = 0; i < arraySize; i++)
        {
            for (int j = 0; j < arraySize; j++)
            {
                float posx = (i - arraySize / 2) * 10.0f;
                float posy = (j - arraySize / 2) * 10.0f;
                TrajList[i, j] = Instantiate(TrajPrefab, new Vector3(posx, posy, 0.0f), quaternion.identity);
                TrajList[i, j].transform.parent = transform;
                TrajList[i, j].GetComponent<MeshRenderer>().material = new Material(TrajMaterial);
                MatList[i, j] = TrajList[i, j].GetComponent<MeshRenderer>().material;
                TexList[i, j] = new Texture2D(1000 / offset, 1000 / offset);
                TexList[i, j].filterMode = FilterMode.Point;
                init_texture2d(TexList[i, j], Color.clear);
                MatList[i, j].mainTexture = TexList[i, j];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        drawOnTexture();
    }
}
