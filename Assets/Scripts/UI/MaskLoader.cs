using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MaskLoader : MonoBehaviour
{
    private Texture2D[] textures = new Texture2D[12];
    public Material mastMat;
    public float intervalTime;
    private float startTime;
    private Vector3 refScale;
    private float minSize = 3.0f / 11.0f;

    public void setSize(float t)
    {
        transform.localScale = refScale * Mathf.Lerp(minSize, 1.0f, t);
    }

    public void setSmallSize()
    {
        StartCoroutine(setSmallSizeIE());
    }

    IEnumerator setSmallSizeIE()
    {
        float st = Time.time;
        for (float t = 0.0f; t < 1.4f; t += 0.02f)
        {
            float tt = t / 1.4f;
            transform.localScale = refScale * Mathf.Lerp(1.0f, minSize, Mathf.Pow(tt, 0.2f));
            yield return new WaitForSeconds(0.02f);
        }
        transform.localScale = refScale * minSize;
    }

    public void setBigSize()
    {
        StartCoroutine(setBigSizeIE());
    }

    IEnumerator setBigSizeIE()
    {
        float st = Time.time;
        for (float t = 0.0f; t < 1.4f; t += 0.02f)
        {
            float tt = t / 1.4f;
            transform.localScale = refScale * Mathf.Lerp(minSize, 1.0f, Mathf.Pow(tt, 0.2f));
            yield return new WaitForSeconds(0.02f);
        }
        transform.localScale = refScale;
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        refScale = transform.localScale;
        for (int i = 0; i < 12; i++)
        {
            // load texture in ##.png format
            textures[i] = Resources.Load<Texture2D>("shielding/zhezhao" + (i+1).ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        int index = (int)((Time.time - startTime) / intervalTime) % 12;
        
        // set main texture of mastMat os textures[index]
        mastMat.SetTexture("_MainTex", textures[index]);
        //transform.position = GameManager.instance.playerController.transform.position;
    }
}
