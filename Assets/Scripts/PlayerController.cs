using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public float horizontalMoveSpeed;
    public LayerMask groundLayer; // 地面的圖層
    public Rigidbody2D rb;
    public float jumpHeight;
    public float gravity;

    [Flags]
    public enum CharacterControllState
    {
        None = 0,
        CanMove = 1<<0,
        CanJump = 1<<1,
    }
    private CharacterControllState currentControllState = CharacterControllState.CanMove | CharacterControllState.CanJump;



    //Input
    private float horizontalInput;

    //Direction
    private Vector3 horizontalMoveDirection;

    //Jump
    private bool isGrounded = false;
    


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();

        HorizontalMove();

        //按下跳躍健
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Jump();
        }

    }
    //Horizon Move
    private void HorizontalMove()
    {
        if ((currentControllState & CharacterControllState.CanMove) == 0) return;

        horizontalInput = Input.GetAxis("Horizontal");
        horizontalMoveDirection = new Vector3(horizontalInput, 0f, 0f).normalized;
        transform.position += horizontalMoveDirection * horizontalMoveSpeed * Time.deltaTime;
    }


    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        isGrounded = hit.collider != null;
    }

    private void Jump()
    {
        if ((currentControllState & CharacterControllState.CanJump) == 0) return;

        //按下按鈕
        //可以控制 "跳躍高度" & "重力加速度"
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity));
    }

    void FixedUpdate()
    {
        //Check is Grounded
        if (!isGrounded)
        {
            rb.AddForce(Vector2.down * gravity);
        }

        //Avoid velocity too fast, then overlap collider.
        if(isGrounded && rb.velocity.y <0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

}
