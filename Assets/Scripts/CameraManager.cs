using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCam;
    public Camera sideMainCam;
    public Camera canvasCam;

    public Transform playerTransform;
    public bool followPlayer = false;
    public float originSize;

    public static CameraManager instance;

    private void Awake()
    {
        // set singleton
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        originSize = mainCam.orthographicSize;
    }

    IEnumerator SetCam(Vector3 lookAt, float size, float time)
    {
        Vector3 curr = mainCam.transform.position;
        float curr_size = mainCam.orthographicSize;
        float bt = Time.time;
        while(Time.time - bt <= time)
        {
            float t = (Time.time - bt) / time;
            mainCam.orthographicSize = curr_size * (1.0f - t) + size * t;
            mainCam.transform.position = curr * (1.0f - t) + lookAt * t;
            yield return null;
        }
        yield return null;
        mainCam.transform.position = lookAt;
        mainCam.orthographicSize = size;
    }

    public void SetCameraPosAndLookat(Vector3 lookAt, float size, float time, bool follow_player=false) {
        lookAt.z = mainCam.transform.position.z;
        followPlayer = follow_player;
        StartCoroutine(SetCam(lookAt, size, time));
        // mainCam.DOOrthoSize(size, time);
        // mainCam.transform.DOMove(lookAt, time);
    }

    public void SetCameraToFollowPlayer(float time)
    {
        SetCameraPosAndLookat(playerTransform.position, originSize, time, true);
    }

    public void SetCameraToFraction(Vector3 lookAt, float fraction, float time, bool follow_player = false)
    {
        SetCameraPosAndLookat(lookAt, originSize * fraction, time, follow_player);
    }

    public void SetCameraToQuater(Vector3 lookat, float time)
    {
        SetCameraPosAndLookat(lookat, originSize / 4.0f, time, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
            mainCam.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, mainCam.transform.position.z);
        sideMainCam.transform.position = mainCam.transform.position;
        sideMainCam.orthographicSize = mainCam.orthographicSize;
        canvasCam.transform.position = mainCam.transform.position;
        // canvasCam.orthographicSize = mainCam.orthographicSize;

        // if press h
        if (Input.GetKeyDown(KeyCode.H)) {
            SetCameraToQuater(Vector3.zero, 1.0f);
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            SetCameraToFollowPlayer(2.0f);
        }

    }
}
