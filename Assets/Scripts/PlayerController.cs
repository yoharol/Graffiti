using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Transform playerTransform;
    public Transform playerAnimTransform;
    public Transform[] playerAnimTransList;
    
    public Animator playerAnimator;
    public Animator playerShadowAnimator;
    public bool running=false;

    private Vector3 _direction;
    public bool borning = false;

    public void setBorn()
    {
        borning = true;
        playerAnimator.Play("neko_born");
    }
    
    

    // Start is called before the first frame update
    void Start()
    {
        // playerAnimator.Play("neko_idle");
        playerShadowAnimator.Play("neko_shadow");
        running = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(borning)
            return;
        // control playerTransform with wasd
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if (move.magnitude > 0)
            _direction = move.normalized;
        playerTransform.position += speed * Time.deltaTime * move;

        if (_direction.x < 0)
            playerTransform.localScale = new Vector3(-1, 1, 1);
        else if (_direction.x > 0)
            playerTransform.localScale = Vector3.one;
        if (move.magnitude < 0.5f)
        {
            running = false;
            playerAnimator.gameObject.SetActive(true);
            if (_direction.y > 0)
            {
                playerAnimator.Play("neko_idle_back");
                playerAnimTransform.localPosition = playerAnimTransList[1].localPosition;
            }
            else if (_direction.y <= 0)
            {
                playerAnimTransform.localPosition = playerAnimTransList[0].localPosition;
                playerAnimator.Play("neko_idle");
            }
        }
        else
        {
            running = true;
            playerAnimator.Play("skate_run");
            playerShadowAnimator.gameObject.SetActive(false);
            playerAnimTransform.localPosition = playerAnimTransList[2].localPosition;
        }
    }
}
