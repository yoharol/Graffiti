using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Transform playerTransform;
    public Transform playerAnimTransform;
    public Transform[] playerAnimTransList;
    public Transform playerBody;
    public GameObject interactBubble;
    
    public Animator playerAnimator;
    public Animator playerShadowAnimator;
    public bool running=false;

    private Vector2 _direction;
    private Rigidbody2D _rigidbody2D;
    public bool borning = false;

    public void setBorn()
    {
        borning = true;
        playerAnimator.Play("neko_born");
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }
    
    public void setInteractBubble(bool active)
    {
        interactBubble.SetActive(active);
    }

    // Start is called before the first frame update
    void Start()
    {
        // playerAnimator.Play("neko_idle");
        playerShadowAnimator.Play("neko_shadow");
        running = false;
        interactBubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(borning)
            return;
        // control playerTransform with wasd
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (move.magnitude > 0)
            _direction = move.normalized;
        Vector2 target = _rigidbody2D.position + speed * Time.deltaTime * (Vector2)move;
        // _rigidbody2D.MovePosition(target);
        _rigidbody2D.AddForce(move * speed * Time.deltaTime * 100.0f);

        if (_direction.x < 0)
            playerBody.localScale = new Vector3(-1, 1, 1);
        else if (_direction.x > 0)
            playerBody.localScale = Vector3.one;
        if (move.magnitude < 0.5f)
        {
            playerShadowAnimator.gameObject.SetActive(true);
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
