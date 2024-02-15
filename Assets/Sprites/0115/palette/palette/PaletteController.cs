using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteController : MonoBehaviour
{
    public bool paletteOpened = false;
    public Color[] colors;
    public int colorIndex = 0;
    public GameObject paletteSliders;
    public GameObject paletteContents;
    RectTransform paletteContentsRectTransform;
    private float velocity = 0.0f;
    public Image colorShow;

    private Color currColor;

    public Color GetActiveColor()
    {
        return colors[colorIndex];
    }
    

    public void openPalette()
    {
        if (paletteOpened)
        {
            paletteSliders.SetActive(false);
            paletteOpened = false;
        }
        else
        {
            paletteOpened = true;
            paletteSliders.SetActive(true);
        }
        // gameObject.SetActive(true);
        // Debug.Log("open palette");
    }

    public void setColorIndex(int index)
    {
        colorIndex = index;
        colorShow.color = colors[index];
        currColor = colors[index];
        colorShow.color = new Color(currColor.r, currColor.g, currColor.b, 1.0f);
        currColor = colorShow.color;
        GameManager.instance.whiteBoard.paintColor = currColor;
    }


    // Start is called before the first frame update
    void Start()
    {
        setColorIndex(2);
        paletteContentsRectTransform = paletteContents.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (paletteOpened)
        {
            // get mouse wheel slide input
            // move palette contents in y axis
            float scroll = Input.mouseScrollDelta.y;
            Vector3 pos = paletteContentsRectTransform.localPosition;
            velocity += scroll * 20.0f * Time.deltaTime;
            velocity *= 0.93f;
            pos.y -= velocity * 15.0f;
            pos.y = Mathf.Clamp(pos.y, -93.92f, 82.89f);
            paletteContentsRectTransform.localPosition = pos;
        }
    }
}
