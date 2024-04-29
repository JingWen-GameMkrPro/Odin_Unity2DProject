using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OldPlayerController;

public class PlayerDatabase: MonoBehaviour
{
    [HideInInspector]
    //角色屬性
    public SD_PlayerAtt.Data PlayerAtt;

    //互動元件，需要調動的私人內部元件
    [SerializeField]
    public SD_PlayerAtt SD_PlayerAtt; //警告：不要修改此變數，這會導致原始資料被更動
    public Rigidbody2D RB;
    public Animator Animator;
    public DetecterManager DetecterManager;

    #region [角色行為權限] 受到外在Debuff可能會用到
    [Flags]
    public enum PlayerPermission
    {
        None = 0,
        CanMove = 1 << 0,
        CanJump = 1 << 1,
        CanAttack = 1 << 2,
        CanDefense = 1 << 3,
    }
    private PlayerPermission playerPermission = PlayerPermission.CanMove | PlayerPermission.CanJump | PlayerPermission.CanAttack | PlayerPermission.CanDefense;
    private Dictionary<PlayerPermission, Coroutine> lastSetTimer = new(); //用於管理目前權限封鎖倒數狀態

    public bool HasPermission(PlayerPermission targetPermission)
    {
        return (playerPermission & targetPermission) != 0;
    }

    public void SetPermisssion(PlayerPermission inputPermission, bool isOpen)
    {
        // 使用按位 OR 設定特定位元
        if (isOpen) playerPermission |= inputPermission;
        // 使用按位 AND 清除特定位元
        else playerPermission &= ~inputPermission;
    }

    public void ClosePermissionForDuration(PlayerPermission inputPermission, int inputSeconds)
    {
        SetPermisssion(inputPermission, false);

        //如果還沒被封鎖
        if (!lastSetTimer.ContainsKey(inputPermission))
        {
            //新增倒數
            lastSetTimer.Add(inputPermission, StartCoroutine(countdownPermission(inputPermission, inputSeconds)));
        }
        //如果正在被封鎖
        else
        {
            //刷新倒數
            StopCoroutine(lastSetTimer[inputPermission]);
            lastSetTimer[inputPermission] = StartCoroutine(countdownPermission(inputPermission, inputSeconds));
        }
    }

    private IEnumerator countdownPermission(PlayerPermission inputPermission, int inputSeconds = 1)
    {
        int currentSeconds = inputSeconds;
        while (currentSeconds > 0)
        {
            yield return new WaitForSeconds(1);
            currentSeconds--;
        }

        //協程結束，刪除KEY值
        if (lastSetTimer.ContainsKey(inputPermission)) lastSetTimer.Remove(inputPermission);
        SetPermisssion(inputPermission, true);
        yield return null;
    }
    #endregion

    #region [角色狀態]
    [Flags]
    public enum PlayerState
    {
        Idle = 1 << 0,
        Moving = 1 << 1,
        Grounding = 1 << 2,
        Attacking = 1 << 3,
        Defensing = 1 << 4,
    }
    private PlayerState playerState = PlayerState.Idle;

    public void SetState(PlayerState targetState, bool isInState, float duration = 999f)
    {
        if (isInState)
        {
            playerState |= targetState;
        }
        else
        {
            playerState &= ~targetState;
        }

        //>999f視同永久設定狀態
        if (duration < 999f)
        {
            StartCoroutine(countdown_State(targetState, duration, isInState));
        }
    }

    private IEnumerator countdown_State(PlayerState targetState, float duration, bool originState)
    {
        float currentSeconds = duration;
        while (currentSeconds > 0)
        {
            yield return new WaitForSeconds(0.05f);
            currentSeconds = currentSeconds - 0.05f;
        }

        //協程結束，返迴狀態
        SetState(targetState, !originState);
        yield return null;
    }

    public void InState(PlayerState targetState)
    {
        playerState |= targetState;
    }

    public void OutState(PlayerState targetState)
    {
        playerState &= ~targetState;
    }

    public bool IsInState(PlayerState targetState)
    {
        return (playerState & targetState) != 0;
    }
    #endregion

    private void Start()
    {
        checkSerializeField();

        //取得類別中的資料結構副本
        PlayerAtt = SD_PlayerAtt.data;

        //計算跳躍相關參數
        UpdateJumpCoefficient();
    }

    //確認SerializeField空值
    private void checkSerializeField()
    {
        if (SD_PlayerAtt == null)
        {
            Debug.LogError("SD_PlayerAtt == null");
        }

        if(RB == null)
        {
            Debug.LogError("rb == null");
        }

        if (Animator == null)
        {
            Debug.LogError("animator == null");
        }

        if (DetecterManager == null)
        {
            Debug.LogError("DetecterManager == null");
        }
    }

    //更新跳躍相關參數
    public void UpdateJumpCoefficient()
    {
        var gravity = (-2 * PlayerAtt.JumpHeight) / (PlayerAtt.JumpToPeakTime * PlayerAtt.JumpToPeakTime);
        RB.gravityScale = Math.Abs(gravity / Physics2D.gravity.y);
        PlayerAtt.JumpStartVelocity = -gravity * (PlayerAtt.JumpToPeakTime);
    }

}
