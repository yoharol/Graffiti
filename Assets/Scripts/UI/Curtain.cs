using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Curtain : MonoBehaviour
{
    Image curtainImage;
    private float startTime;
    private float period = 1.0f;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    
    public void setAlpha(float a)
    {
        curtainImage.color = new Color(curtainImage.color.r, curtainImage.color.g, curtainImage.color.b, a);
    }

    public void setFadeIn(float t)
    {
        period = t;
        StartCoroutine(FadeIn(t));
    }
    
    public void setFadeOut(float t)
    {
        period = t;
        StartCoroutine(FadeOut(t));
    }
    
    IEnumerator FadeOut(float t)
    {
        isFadingOut = true;
        startTime = Time.time;
        yield return new WaitForSeconds(t);
        isFadingOut = false;
    }
    
    IEnumerator FadeIn(float t)
    {
        isFadingIn = true;
        startTime = Time.time;
        yield return new WaitForSeconds(t);
        isFadingIn = false;
    }

    private void Awake()
    {
        curtainImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (isFadingIn)
        {
            float t = (Time.time - startTime)/period;
            float a = Mathf.Lerp(1f, 0f, t*t);
            setAlpha(a);
        }
        else if (isFadingOut)
        {
            float t = (Time.time - startTime)/period;
            float a = Mathf.Lerp(0f, 1f, Mathf.Sqrt(t));
            setAlpha(a);
        }
    }
}
