using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSticker : MonoBehaviour
{
    LoadButton loadButton;
    public GameObject sticker;
    public GameObject lightSource;

    public BoxCollider2D click_region;

    public bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = sticker.transform.position;
        sticker = Instantiate(GameManager.instance.playerIcon);
        sticker.SetActive(false);
        sticker.transform.position = pos;
        loadButton = GetComponent<LoadButton>();
    }

    // Update is called once per frame
    void Update()
    {
        // if E is pressed and the player is in the trigger area
        if (Input.GetKeyDown(KeyCode.E) && triggered)
        {
            
        }
        if (Input.GetMouseButtonDown(0) && triggered)
        {
            // Convert mouse position to a ray
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Bounds bounds = click_region.bounds;
            Debug.Log(clickPosition);
            Debug.Log(bounds.center);
            Debug.Log(bounds.size);
            Debug.Log(click_region.bounds.size);
            Debug.Log(bounds.Contains(clickPosition));
            if (bounds.Contains(clickPosition))
            {
                UnityEngine.Debug.Log("Clicked");  
                // GameManager.instance.maskLoader.setBigSize();
                loadButton.playDisappearAnimation();
                triggered = false;
                this.enabled = false;
                sticker.SetActive(true);
                lightSource.SetActive(true);
                GetComponent<BoxCollider2D>().enabled = false;

            }
        }


    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            triggered = true;
            loadButton.playAppearAnimation();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            triggered = false;
            loadButton.playDisappearAnimation();
        }
    }
}
