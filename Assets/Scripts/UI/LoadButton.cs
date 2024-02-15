using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    private Animator buttonAnimator;

    public void playAppearAnimation()
    {
        buttonAnimator.Play("button_show");
    }
    
    public void playDisappearAnimation()
    {
        buttonAnimator.Play("button_disappear");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        buttonAnimator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            playAppearAnimation();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            playDisappearAnimation();
        }
        
    }
}
