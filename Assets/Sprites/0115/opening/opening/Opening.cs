using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{
    // there are five objects in the opening scene
    // activate them one by one and play animation automatically
    // When one animation is finished, activate the next object
    // and play the next animation
    
    public GameObject[] objects;
    public Animator[] animators;
    public int index = 0;
    public bool finished = false;
    public bool started = false;
    public bool playing = false;
    Animator currentAnimator;
    
    public void playNextAnimation()
    {
        if (index < objects.Length)
        {
            currentAnimator = animators[index];
            objects[index].SetActive(true);
            if (index > 0)
            {
                objects[index - 1].SetActive(false);
            }
            // string animationName = "scene" + index.ToString();
            // animators[index].Play("animationName");
            index += 1;
        }
        else
        {
            objects[index-1].SetActive(false);
            finished = true;
            // load next scene by scene index + 1
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
    public void startOpening()
    {
        started = true;
        playNextAnimation();
    }

    private void Start()
    {
        startOpening();
    }

    private void Update()
    {
        // play the next animation when click left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            if (started && !finished)
            {
                playNextAnimation();
            }
        }
        
        // play the next animation when the current animation is finished
        if (started && !finished && !playing)
        {
            if (currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                playing = false;
                playNextAnimation();
            }
        }
        
        //load next scene if click right mouse button for 0.5 second
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
}
