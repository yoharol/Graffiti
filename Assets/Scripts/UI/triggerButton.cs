using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class triggerButton : MonoBehaviour
{
    LoadButton loadButton;
    public GameObject sticker;

    public bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = sticker.transform.position;
        sticker = Instantiate(GameManager.instance.playerIcon);
        sticker.SetActive(false);
        sticker.transform.position = pos;
        loadButton = GetComponent<LoadButton>();
    }

    // Update is called once per frame
    void Update()
    {
        // if E is pressed and the player is in the trigger area
        if (Input.GetKeyDown(KeyCode.E) && triggered)
        {
            GameManager.instance.maskLoader.setBigSize();
            loadButton.playDisappearAnimation();
            triggered = false;
            this.enabled = false;
            sticker.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
        }


    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            triggered = true;
            loadButton.playAppearAnimation();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            triggered = false;
            loadButton.playDisappearAnimation();
        }
    }
}
