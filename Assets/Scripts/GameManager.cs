using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    //singleton of GameManager, hide in inspector
    [HideInInspector] public static GameManager instance;
    public Curtain curtain;
    public PlayerController playerController;
    public PlayerTrajectory playerTrajectory;
    public CinemachineBrain virtualCamera;
    
    public Camera[] cameras;

    private void Awake()
    {
        instance = this;
        foreach (var camera in cameras)
        {
            camera.transparencySortMode = TransparencySortMode.CustomAxis;
            camera.transparencySortAxis = new Vector3(0, 1, 0);
        }
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
