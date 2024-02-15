using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoxPaint : MonoBehaviour{
    public bool triggered = false;
    private bool waitingForPainting = false;
    private SpriteRenderer boardSticker;
    public SpriteRenderer presetSprite;
    public SpriteRenderer face;
    public float localX;
    
    IEnumerator startPainting()
    {
        GameManager.instance.playerController.enabled = false;
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(GameManager.instance.whiteBoard.StartPaintingIE());
        yield return new WaitForSeconds(0.5f);
    }

    private void Start()
    {
        boardSticker = face;
    }

    private void Update()
    {
        if (waitingForPainting)
        {
            if (GameManager.instance.whiteBoard.finished == true)
            {
            }
        }
        else if (triggered && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(startPainting());
            GameManager.instance.whiteBoard.targetSpriteRenderer = boardSticker;
            GameManager.instance.whiteBoard.preset.GetComponent<SpriteRenderer>().sprite = presetSprite.sprite;
            Vector3 localPos = GameManager.instance.whiteBoard.preset.transform.localPosition;
            GameManager.instance.whiteBoard.preset.transform.localPosition = new Vector3(localX, localPos.y, localPos.z);
            waitingForPainting = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            triggered = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            triggered = false;
        }
    }
    
}
