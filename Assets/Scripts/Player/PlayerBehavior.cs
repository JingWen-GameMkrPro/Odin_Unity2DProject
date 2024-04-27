using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using static OldPlayerController;

//實作角色各種行為
public class PlayerBehavior : MonoBehaviour
{
    [SerializeField]
    public PlayerDatabase PlayerDatabase;

    private void Start()
    {
        checkSerializeField();
    }

    //確認SerializeField空值
    private void checkSerializeField()
    {
        if (PlayerDatabase == null)
        {
            Debug.LogError("Player Attributer == null");
        }
    }

    public void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Map"));
        //一踩到地板就要緊急煞車，避免穿透
        if (!PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Grounding) && hit.collider != null)
        {
            PlayerDatabase.rb.velocity = new Vector2(PlayerDatabase.rb.velocity.x, 0);
        }
        PlayerDatabase.SetState(PlayerDatabase.PlayerState.Grounding, hit.collider != null);
    }

    public void UpdateaAnimatorState()
    {
        PlayerDatabase.animator.SetBool("isJumping", !PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Grounding));
        PlayerDatabase.animator.SetFloat("xSpeed", Math.Abs(PlayerDatabase.rb.velocity.x));
        PlayerDatabase.animator.SetBool("isAttacking", PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Attacking));
        PlayerDatabase.animator.SetBool("isDefensing", PlayerDatabase.IsInState(PlayerDatabase.PlayerState.Defensing));
    }

    public void TryHorizontalMove()
    {
        if(!PlayerDatabase.HasPermission(PlayerDatabase.PlayerPermission.CanMove))
        {
            return;
        }

        //控制位移
        var horizontalInput = Input.GetAxis("Horizontal");
        horizontalInput = Math.Clamp(horizontalInput * PlayerDatabase.playerAtt.HorizontalAcceleration, -1, 1);
        PlayerDatabase.rb.velocity = new Vector2(horizontalInput * PlayerDatabase.playerAtt.HorizontalSpeed, PlayerDatabase.rb.velocity.y);

        //控制方向
        if (PlayerDatabase.rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (PlayerDatabase.rb.velocity.x < 0)
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

    //扣血操作
    public void DoReduceHP(float reduceValue)
    {

    }

    //造成傷害操作
    public void DoApplyDamage(float reduceValue)
    {

    }
}
