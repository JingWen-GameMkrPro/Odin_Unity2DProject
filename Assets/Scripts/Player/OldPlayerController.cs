using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Android;
//using UnityEngine.Windows;

public class OldPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public float horizontalMoveSpeed;
    public float Acceleration;
    public LayerMask groundLayer; // 地面的圖層
    public Rigidbody2D rb;

    [InspectorLabel("Jump")]
    public float jumpHeight = 5f;
    public float jumpUpTime = 0.5f;
    private float jumpStartVelocity;

    //Animator
    public Animator animator;

    //System
    public InteractManager currentInteractManager;

    //Collider
    public Collider2D currentHitBox;
    public Collider2D currentAttackBox;

    //Move
    private float horizontalInput;

    //Character Permission
    [Flags]
    public enum CharacterPermission
    {
        None = 0,
        CanMove = 1<<0,
        CanJump = 1<<1,
        CanAttack = 1<<2,
        CanDefense = 1<<3,
    }
    private CharacterPermission currentPermission = CharacterPermission.CanMove | CharacterPermission.CanJump | CharacterPermission.CanAttack | CharacterPermission.CanDefense;
    private Dictionary<CharacterPermission, Coroutine> lastSetTimer = new (); //用於管理目前權限封鎖倒數狀態

    //Chracter State
    [Flags]
    public enum CharacterState
    {
        Idle = 1<<0,
        Moving = 1<<1,
        Grounding = 1<<2,
        Attacking = 1<<3,
        Defensing = 1<<4,
    }
    private CharacterState currentState;
    
    void DebugMessage()
    {
        //Debug.Log("Current State: " + currentState.ToString());
        //Debug.Log(rb.velocity);
    }

    void Start()
    {
        setState(CharacterState.Idle, true);
        calculateJumpCoefficient();
    }

    // Update is called once per frame
    void Update()
    {
        DebugMessage();

        //隨時確認是否在地板
        checkGrounded();

        //水平移動輸入
        if(hasPermission(CharacterPermission.CanMove))
        {
            horizontalMove();
        }
        
        //跳躍輸入，如果isGrounded = 1才行
        if (Input.GetKeyDown(KeyCode.W) && isInState(CharacterState.Grounding) && hasPermission(CharacterPermission.CanJump))
        {
            jump();
        }

        //攻擊輸入
        if(Input.GetKeyDown(KeyCode.J) && !isInState(CharacterState.Attacking) && hasPermission(CharacterPermission.CanAttack))
        {
            Attack();
        }

        //防禦輸入
        if (Input.GetKeyDown(KeyCode.K) && !isInState(CharacterState.Defensing) && hasPermission(CharacterPermission.CanDefense))
        {
            Defense();
        }

        //Animation
        setAnimationState();
    }

    void FixedUpdate()
    {
        
    }

    private void Attack()
    {
        setState(CharacterState.Attacking, true, 0.2f);
        currentInteractManager.Damage(10, "AttackRange");
    }

    private void Defense()
    {
        setState(CharacterState.Defensing, true, 0.5f);
    }

    private void setAnimationState()
    {
        animator.SetBool("isJumping", !isInState(CharacterState.Grounding));
        animator.SetFloat("xSpeed", Math.Abs(rb.velocity.x));
        animator.SetBool("isAttacking", isInState(CharacterState.Attacking));
        animator.SetBool("isDefensing", isInState(CharacterState.Defensing));
    }

    //Horizon Move
    private void horizontalMove()
    {
        //Transformation
        //horizontalInput = Input.GetAxis("Horizontal");
        horizontalInput = Math.Clamp(horizontalInput * Acceleration, -1, 1);
        rb.velocity = new Vector2(horizontalInput * horizontalMoveSpeed, rb.velocity.y);

        //Direction
        if(rb.velocity.x>0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(rb.velocity.x<0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //State
        if (horizontalInput == 0)
        {
            setState(CharacterState.Moving, false);
            setState(CharacterState.Idle, true);
        }
        else
        {
            setState(CharacterState.Moving, true);
            setState(CharacterState.Idle, false);
        }
    }

    private void checkGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        //一踩到地板就要緊急煞車，避免穿透
        if(!isInState(CharacterState.Grounding) && hit.collider != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        setState(CharacterState.Grounding, hit.collider != null);
    }

    private void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpStartVelocity);
    }

    //用於角色被封鎖權限之類的Debuff，像是移動、跳躍、攻擊
    private void closePermissionForDuration(CharacterPermission inputPermission, int inputSeconds)
    {
        setPermisssion(inputPermission, false);

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
            yield return new WaitForSeconds(1);
            currentSeconds--;
        }

        //協程結束，刪除KEY值
        if (lastSetTimer.ContainsKey(inputPermission)) lastSetTimer.Remove(inputPermission);
        setPermisssion(inputPermission, true);
        yield return null;
    }

    //設定權限
    private void setPermisssion(CharacterPermission inputPermission, bool isOpen)
    {
        // 使用按位 OR 設定特定位元
        if (isOpen) currentPermission |= inputPermission;
        // 使用按位 AND 清除特定位元
        else currentPermission &= ~inputPermission;
    }

    private bool hasPermission(CharacterPermission targetPermission)
    {
        return (currentPermission & targetPermission) != 0;
    }

    private void setState(CharacterState targetState, bool isInState, float duration = 999f)
    {
        if (isInState)
        {
            currentState |= targetState;
        }
        else
        {
            currentState &= ~targetState;
        }

        //>999f視同永久設定狀態
        if(duration<999f)
        {
            StartCoroutine(Countdown_State (targetState, duration, isInState));
        }
    }

    IEnumerator Countdown_State(CharacterState targetState, float duration, bool originState)
    {
        float currentSeconds = duration;
        while (currentSeconds > 0)
        {
            yield return new WaitForSeconds(0.05f);
            currentSeconds = currentSeconds - 0.05f;
        }

        //協程結束，返迴狀態
        setState(targetState, !originState);
        yield return null;
    }

    private void inState(CharacterState targetState)
    {
        currentState |= targetState;
    }

    private void outState(CharacterState targetState)
    {
        currentState &= ~targetState;
    }

    private bool isInState(CharacterState targetState)
    {
        return (currentState & targetState) != 0;
    }

    private void calculateJumpCoefficient()
    {
        var gravity = (-2 * jumpHeight) / (jumpUpTime * jumpUpTime);
        rb.gravityScale = Math.Abs(gravity / Physics2D.gravity.y);
        jumpStartVelocity = -gravity * jumpUpTime;

    }
}
