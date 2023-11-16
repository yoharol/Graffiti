using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton of GameManager, hide in inspector
    [HideInInspector] public static GameManager instance;
    public Curtain curtain;
    public PlayerController playerController;
    public PlayerTrajectory playerTrajectory;
    public CinemachineBrain virtualCamera;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
