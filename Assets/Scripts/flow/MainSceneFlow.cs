using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneFlow : MonoBehaviour
{
    
    IEnumerator setPlayerBorn()
    {
        GameManager.instance.playerController.gameObject.SetActive(false);
        GameManager.instance.playerController.playerShadowAnimator.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        GameManager.instance.playerController.gameObject.SetActive(true);
        GameManager.instance.playerController.setBorn();
        yield return new WaitForSeconds(2.0f);

        Vector3 vel = Vector3.zero;
        Vector3 targetPos = GameManager.instance.playerController.transform.position;
        targetPos.z = Camera.main.transform.position.z;
        for (float t = 0; t < 1.5f; t+=Time.deltaTime)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position,
                targetPos, ref vel, 0.3f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GameManager.instance.playerController.playerAnimator.gameObject.SetActive(true);
        GameManager.instance.playerController.playerShadowAnimator.gameObject.SetActive(true);
        GameManager.instance.playerController.enabled = true;
        GameManager.instance.playerController.borning = false;
        GameManager.instance.virtualCamera.enabled = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.playerTrajectory.gameObject.SetActive(false);
        GameManager.instance.curtain.setFadeIn(2.0f);
        GameManager.instance.playerController.enabled = false;
        StartCoroutine(setPlayerBorn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
