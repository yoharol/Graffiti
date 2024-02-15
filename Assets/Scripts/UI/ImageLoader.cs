using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    public int imgCount;
    public string filepath;
    public Sprite[] images;
    SpriteRenderer image;

    public bool loop = false;
    public float interval = 0.1f;

    private float startTime;
    
    
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        image = GetComponent<SpriteRenderer>();
        images = new Sprite[imgCount];
        for (int i = 0; i < imgCount; i++)
        {
            images[i] = Resources.Load<Sprite>(filepath + i.ToString());
        }

    }

    // Update is called once per frame
    void Update()
    {
        int index = (int)((Time.time - startTime) / interval);
        if (loop) index = index % imgCount;
        else if (index >= imgCount) index = imgCount - 1;
        image.sprite = images[index];
    }
}
