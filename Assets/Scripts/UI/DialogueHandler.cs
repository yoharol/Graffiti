using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject dialogueBox;
    
    public string[] dialogues;
    public int dialogueIndex = 0;
    
    public void setText(string text)
    {
        this.text.text = text;
    }
    
    IEnumerator setTextIE(string text)
    {
        this.text.text = "";
        Debug.Log(text);
        foreach (var c in text)
        {
            this.text.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    public void setDialogues(string[] dialogues)
    {
        this.dialogues = dialogues;
        dialogueIndex = 0;
        dialogueBox.SetActive(true);
        // setText(dialogues[dialogueIndex]);
        StartCoroutine(setTextIE(dialogues[dialogueIndex]));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((dialogues != null) && (dialogueIndex < dialogues.Length) && Input.GetKeyDown(KeyCode.E))
        {
            dialogueIndex += 1;
            if (dialogueIndex < dialogues.Length)
            {
                StartCoroutine(setTextIE(dialogues[dialogueIndex]));
                // setText(dialogues[dialogueIndex]);
            }
            else
            {
                dialogueBox.SetActive(false);
                dialogues = null;
                dialogueIndex = -1;
            }
        }
    }
}
