using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OldPlayerController;

public class InnerInteracterPlayer : MonoBehaviour
{
    [SerializeField]
    PlayerDatabase PlayerDatabase;

    [HideInInspector]
    public GameObject Causer;

    private void Start()
    {
        checkSerializeField();

        Causer = transform.root.gameObject;
    }

    //確認SerializeField空值
    private void checkSerializeField()
    {
        if (PlayerDatabase == null)
        {
            Debug.LogError("PlayerDatabase == null");
        }
    }

    private void Update()
    {
        checkGrounded();

        updateaAnimatorState();

        receiveInput();
    }

    private void receiveInput()
    {
        //移動輸入
        tryHorizontalMove();

        //跳躍輸入，如果isGrounded = 1才行
        if (Input.GetKeyDown(KeyCode.W))
        {
            tryJump();
        }

        //攻擊輸入
        if (Input.GetKeyDown(KeyCode.J))
        {
            tryAttack();
        }

        //防禦輸入
        if (Input.GetKeyDown(KeyCode.K))
        {
            tryDefense();
        }

    }

    private void tryHorizontalMove()
    {
        if (!PlayerDatabase.HasPermission(PlayerDatabase.PlayerPermission.CanMove))
        {
            return;
        }

        //控制位移
        var horizontalInput = Input.GetAxis("Horizontal");
        horizontalInput = Math.Clamp(horizontalInput * PlayerDatabase.playerAtt.HorizontalAcceleration, -1, 1);
        PlayerDatabase.RB.velocity = new Vector2(horizontalInput * PlayerDatabase.playerAtt.HorizontalSpeed, PlayerDatabase.RB.velocity.y);

        //控制方向
        if (PlayerDatabase.RB.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (PlayerDatabase.RB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //控制動畫狀態
        if (horizontalInput == 0)
        {
            PlayerDatabase.SetState(PlayerDatabase.PlayerState.Moving, false);
            PlayerDatabase.SetState(PlayerDatabase.PlayerState.Idle, true);
        }
        else
        {
            PlayerDatabase.SetState(PlayerDatabase.PlayerState.Moving, true);
            PlayerDatabase.SetState(PlayerDatabase.PlayerState.Idle, false);
        }
    }

    private void tryJump()
    {
        if(!PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Grounding) || !PlayerDatabase.HasPermission(PlayerDatabase.PlayerPermission.CanJump))
        {
            return;
        }

        PlayerDatabase.RB.velocity = new Vector2(PlayerDatabase.RB.velocity.x, PlayerDatabase.playerAtt.JumpStartVelocity);
    }

    private void tryAttack()
    {
        if(PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Attacking) || !PlayerDatabase.HasPermission(PlayerDatabase.PlayerPermission.CanAttack))
        {
            return;
        }

        PlayerDatabase.SetState(PlayerDatabase.PlayerState.Attacking, true, 0.2f);

        if(PlayerDatabase.DetecterManager.DetecterRecorderList.ContainsKey("AttackDetecter"))
        {
            Debug.Log("Success Attacking!");
            foreach(var target in PlayerDatabase.DetecterManager.DetecterRecorderList["AttackDetecter"])
            {
                target.ReduceHP(Causer, 10);
            }
        }
    }

    private void tryDefense()
    {
        if (PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Defensing) || !PlayerDatabase.HasPermission(PlayerDatabase.PlayerPermission.CanDefense))
        {
            return;
        }

        PlayerDatabase.SetState(PlayerDatabase.PlayerState.Defensing, true, 0.5f);

    }

    private void checkGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Map"));
        //一踩到地板就要緊急煞車，避免穿透
        if (!PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Grounding) && hit.collider != null)
        {
            PlayerDatabase.RB.velocity = new Vector2(PlayerDatabase.RB.velocity.x, 0);
        }
        PlayerDatabase.SetState(PlayerDatabase.PlayerState.Grounding, hit.collider != null);
    }

    private void updateaAnimatorState()
    {
        PlayerDatabase.Animator.SetBool("isJumping", !PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Grounding));
        PlayerDatabase.Animator.SetFloat("xSpeed", Math.Abs(PlayerDatabase.RB.velocity.x));
        PlayerDatabase.Animator.SetBool("isAttacking", PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Attacking));
        PlayerDatabase.Animator.SetBool("isDefensing", PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Defensing));
    }


}
