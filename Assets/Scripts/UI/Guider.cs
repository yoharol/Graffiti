using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guider : MonoBehaviour
{
    public GameObject face1;
    public GameObject face2;
    public GameObject face3;
    public Animator dialogueAnimator;
    public LoadButton loadButton;
    public bool triggered = false;
    private int state = 1;

    public void setFace2()
    {
        face1.SetActive(false);
        face2.SetActive(true);
        dialogueAnimator.Play("nicejob");
    }

    public void setFace3()
    {
        face2.SetActive(false);
        face3.SetActive(true);
        dialogueAnimator.Play("goright");
    }

    private void Update()
    {
        if (triggered && Input.GetKeyDown(KeyCode.E))
        {
            state += 1;
            if (state == 2)
            {
                setFace2();
            }

            if (state == 3)
            {
                setFace3();
                GetComponent<BoxCollider2D>().enabled = false;
                loadButton.playDisappearAnimation();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            loadButton.playAppearAnimation();
            triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            loadButton.playDisappearAnimation();
            triggered = false;
        }
    }
}
