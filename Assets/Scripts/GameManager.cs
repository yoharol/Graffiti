using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    //singleton of GameManager, hide in inspector
    [HideInInspector] public static GameManager instance;
    public Curtain curtain;
    public PlayerController playerController;
    public PlayerTrajectory playerTrajectory;
    public CinemachineBrain virtualCamera;
    public MaskLoader maskLoader;
    public WhiteBoard whiteBoard;
    public DialogueHandler dialogueHandler;
    public PaletteController palette;

    public UniversalRendererData renderData;
    public LayerMask mainCameraLayerMask;
    public VolumeProfile postProcessData;
    private ColorAdjustments colorAdjust;

    public TMPro.TextMeshProUGUI stickerCount;

    public GameObject playerIcon;
    
    public Camera[] cameras;
    public bool whiteBoardMode = false;

    public void SetStickerCount(int count, int max_count)
    {
        stickerCount.text = count.ToString() + " / " + max_count.ToString();
    }

    private void Awake()
    {
        renderData.transparentLayerMask = mainCameraLayerMask;
        postProcessData.TryGet<ColorAdjustments>(out colorAdjust);
        colorAdjust.postExposure.value = (-3.0f);
        //set resolution
        Screen.SetResolution(1280, 720, false);
        instance = this;
        foreach (var camera in cameras)
        {
            camera.transparencySortMode = TransparencySortMode.CustomAxis;
            camera.transparencySortAxis = new Vector3(0, 1, 0);
        }
        playerIcon = GameObject.Find("player_icon");
        if (playerIcon == null)
        {
            playerIcon = transform.Find("backup").gameObject;
        }
        playerIcon.transform.localScale = playerIcon.transform.localScale * 0.5f;
        playerIcon.SetActive(false);

        SetStickerCount(0, 20);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        renderData.transparentLayerMask = ~0;
        colorAdjust.postExposure.value = 0.0f;
    }
}
