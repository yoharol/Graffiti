using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class face : MonoBehaviour
{
    public bool triggered;
    private Animator faceAnimator;
    private int state = 0;
    private bool dialogueSent = false;
    private bool alreadyFirstPaint = false;
    private bool alreadyTriggerBubble = false;

    public Image icon;
    
    private bool waitingForPainting = false;
    public GameObject gettingDialogue;
    public GameObject thinking;
    public SpriteRenderer presetFace;

    private PaintTarget paintTarget;

    IEnumerator startPainting()
    {
        GameManager.instance.whiteBoard.targetSpriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.instance.whiteBoard.preset.GetComponent<SpriteRenderer>().sprite = presetFace.sprite;
        GameManager.instance.playerController.setInteractBubble(false);
        StartCoroutine(GameManager.instance.whiteBoard.StartWhiteBoardIE());
        yield return new WaitForSeconds(0.5f);
        thinking.SetActive(true);
        // GameManager.instance.playerController.enabled = false;
        alreadyFirstPaint = true;
        // StartCoroutine(GameManager.instance.whiteBoard.StartPaintingIE(paintTarget));
        faceAnimator.enabled = false;
    }

    IEnumerator dialogues()
    {
        string[] diags = new string[5];
        diags[0] = "Ah la la Finally";
        diags[1] = "My sweet voice is back!";
        diags[2] = "How's you day, my friend?";
        diags[3] = "Wait, you cannot speak yet?";
        diags[4] = "Take this, and we can talk!";
        GameManager.instance.dialogueHandler.setDialogues(diags);
        while (GameManager.instance.dialogueHandler.dialogueIndex != -1)
        {
            yield return null;
        }
        gettingDialogue.SetActive(true);
        // wait until key E is pressed
        yield return new WaitForSeconds(0.5f);
        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }
        gettingDialogue.SetActive(false);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        paintTarget = GetComponent<PaintTarget>();
        faceAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueSent)
        {
        }
        else if (waitingForPainting)
        {
            if (GameManager.instance.whiteBoard.finished == true)
            {
                icon.sprite = GetComponent<SpriteRenderer>().sprite;
                icon.SetNativeSize();
                thinking.SetActive(false);
                StartCoroutine(dialogues());
                dialogueSent = true;
            }
        }
        else if (triggered && Input.GetKeyDown(KeyCode.E))
        {
            state += 1;
            if (state == 1)
                faceAnimator.Play("zhendong");
            else if (state == 2)
            {
                faceAnimator.Play("weiqu");
                StartCoroutine(startPainting());
                waitingForPainting = true;
            }
        }

        
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && !alreadyFirstPaint)
        {
            triggered = true;
            if (!alreadyTriggerBubble)
            {
                alreadyTriggerBubble = true;
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
            alreadyTriggerBubble = false;
        }
    }
}
