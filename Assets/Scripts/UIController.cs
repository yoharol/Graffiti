using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public PlayerController controller;
    public WhiteBoard whiteBoard;

    public Image[] statusButton;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void ClearButton()
    {
        foreach (var b in statusButton)
        {
            b.enabled = false;
        }
        for( int i=0; i<3; i++)
            statusButton[i].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        ClearButton();
        if (whiteBoard.painting)
        {
            statusButton[1].enabled = false;
            statusButton[4].enabled = true;
        }
        else if (controller.running)
        {
            statusButton[2].enabled = false;
            statusButton[5].enabled = true;
        }
        else
        {
            statusButton[0].enabled = false;
            statusButton[3].enabled = true;
        }
        
    }
}
