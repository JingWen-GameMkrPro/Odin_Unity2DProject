using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Android;
//using UnityEngine.Windows;

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
    public enum CharacterPermission
    {
        None = 0,
        CanMove = 1<<0,
        CanJump = 1<<1,
    }
    private CharacterPermission currentPermission = CharacterPermission.CanMove | CharacterPermission.CanJump;
    private Dictionary<CharacterPermission, Coroutine> lastSetTimer = new (); //用於管理目前權限封鎖倒數狀態

    [Flags]
    public enum CharacterState
    {
        None = 0,
        isMove = 1 << 0,
        isGrounded = 1 << 1,
    }
    private CharacterState currentState = CharacterState.isMove | CharacterState.isGrounded;


    //Move
    private float horizontalInput;
    private Vector3 horizontalMoveDirection;

    //Jump
    //private bool isGrounded = false;
    


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("Current Permission State: " + Convert.ToString((int)currentState, 2).PadLeft(2, '0'));

        //隨時確認是否在地板
        CheckGrounded();

        //水平移動輸入
        HorizontalMove();

        //跳躍輸入，如果isGrounded = 1才行
        if (Input.GetKeyDown(KeyCode.W) && (currentState & CharacterState.isGrounded) !=0)
        {
            Jump();
        }

    }
    //Horizon Move
    private void HorizontalMove()
    {
        //確認權限
        if ((currentPermission & CharacterPermission.CanMove) == 0) return;

        horizontalInput = Input.GetAxis("Horizontal");
        horizontalMoveDirection = new Vector3(horizontalInput, 0f, 0f).normalized;
        transform.position += horizontalMoveDirection * horizontalMoveSpeed * Time.deltaTime;
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        bool isGrounded = hit.collider != null;
        Debug.Log(isGrounded);
        SetState(CharacterState.isGrounded, isGrounded);
    }

    private void Jump()
    {
        //確認權限
        if ((currentPermission & CharacterPermission.CanJump) == 0) return;

        //按下按鈕
        //可以控制 "跳躍高度" & "重力加速度"
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity));
    }

    void FixedUpdate()
    {
        //確認是否在地板
        if ((currentState | CharacterState.isGrounded) != 0)
        {
            rb.AddForce(Vector2.down * gravity);
        }

        //當下墜後踩到地板的瞬間，必須盡快煞車，避免穿透地板
        if((currentState | CharacterState.isGrounded) == 0 && rb.velocity.y <0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

    //用於角色被封鎖權限之類的Debuff，像是移動、跳躍、攻擊
    public void ClosePermissionForDuration(CharacterPermission inputPermission, int inputSeconds)
    {
        SetPermisssion(inputPermission, false);

        //如果還沒被封鎖
        if(!lastSetTimer.ContainsKey(inputPermission))
        {
            //新增倒數
            lastSetTimer.Add(inputPermission, StartCoroutine(Countdown_Permission(inputPermission, inputSeconds)));
        }
        //如果正在被封鎖
        else
        {
            //刷新倒數
            StopCoroutine(lastSetTimer[inputPermission]);
            lastSetTimer[inputPermission] = StartCoroutine(Countdown_Permission(inputPermission, inputSeconds));
        }


    }

    //倒數計時
    IEnumerator Countdown_Permission(CharacterPermission inputPermission, int inputSeconds = 1)
    {
        int currentSeconds = inputSeconds;
        while(currentSeconds>0)
        {
            currentSeconds--;
            yield return new WaitForSeconds(1);
        }

        //協程結束，刪除KEY值
        if (lastSetTimer.ContainsKey(inputPermission)) lastSetTimer.Remove(inputPermission);
        SetPermisssion(inputPermission, true);
        yield return null;
    }

    //設定權限
    private void SetPermisssion(CharacterPermission inputPermission, bool isOpen)
    {
        // 使用按位 OR 設定特定位元
        if (isOpen) currentPermission |= inputPermission;
        // 使用按位 AND 清除特定位元
        else currentPermission &= ~inputPermission;
    }

    private void SetState(CharacterState inputState, bool isOpen)
    {
        // 使用按位 OR 設定特定位元
        if (isOpen) currentState |= inputState;
        // 使用按位 AND 清除特定位元
        else currentState &= ~inputState;
    }

}
