using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreePaintBoard : MonoBehaviour
{
    public bool triggered = false;
    private bool waitingForPainting = false;
    private SpriteRenderer boardSticker;
    private bool firstPainted;
    private bool bubbleTriggered;
    
    IEnumerator startPainting()
    {
        firstPainted = true;
        GameManager.instance.playerController.setInteractBubble(false);
        GameManager.instance.playerController.enabled = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(GameManager.instance.whiteBoard.StartPaintingIE());
        yield return new WaitForSeconds(0.5f);
    }

    private void Start()
    {
        boardSticker = transform.Find("PaintBoard").GetComponent<SpriteRenderer>();
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
            GameManager.instance.whiteBoard.preset.GetComponent<SpriteRenderer>().sprite = null;
            waitingForPainting = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            triggered = true;
            if (!bubbleTriggered && !firstPainted)
            {
                bubbleTriggered = true;
                GameManager.instance.playerController.setInteractBubble(true);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            triggered = false;
            GameManager.instance.playerController.setInteractBubble(false);
        }
    }
}
