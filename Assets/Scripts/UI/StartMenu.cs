using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour
{
    private Camera mainCam;
    public Animator screenAnimator;
    
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(5.0f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // geet mouse position
            Vector2 mousePos = Input.mousePosition;
            var rayHit = Physics2D.GetRayIntersection(mainCam.ScreenPointToRay(mousePos));
            if (!rayHit.collider) return;
            if (rayHit.collider.gameObject.name == "Start")
            {
                // load scene 1
                screenAnimator.Play("startgame");
                StartCoroutine(LoadScene());
            }
            else if (rayHit.collider.gameObject.name == "Exit")
            {
                Application.Quit();
            }
        }
    }
}
