using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OldPlayerController;

public class PlayerDatabase: MonoBehaviour
{
    [HideInInspector]
    //�����ݩ�
    public SD_PlayerAtt.Data PlayerAtt;

    //���ʤ���A�ݭn�հʪ��p�H��������
    [SerializeField]
    public SD_PlayerAtt SDPlayerAtt; //ĵ�i�G���n�ק惡�ܼơA�o�|�ɭP��l��ƳQ���
    public Rigidbody2D RB;
    public Animator Animator;
    public DetecterManager DetecterManager;

    #region [����欰�v��] ����~�bDebuff�i��|�Ψ�
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
    private Dictionary<PlayerPermission, Coroutine> lastSetTimer = new(); //�Ω�޲z�ثe�v������˼ƪ��A

    public bool HasPermission(PlayerPermission targetPermission)
    {
        return (playerPermission & targetPermission) != 0;
    }

    public void SetPermisssion(PlayerPermission inputPermission, bool isOpen)
    {
        // �ϥΫ��� OR �]�w�S�w�줸
        if (isOpen) playerPermission |= inputPermission;
        // �ϥΫ��� AND �M���S�w�줸
        else playerPermission &= ~inputPermission;
    }

    public void ClosePermissionForDuration(PlayerPermission inputPermission, int inputSeconds)
    {
        SetPermisssion(inputPermission, false);

        //�p�G�٨S�Q����
        if (!lastSetTimer.ContainsKey(inputPermission))
        {
            //�s�W�˼�
            lastSetTimer.Add(inputPermission, StartCoroutine(countdownPermission(inputPermission, inputSeconds)));
        }
        //�p�G���b�Q����
        else
        {
            //��s�˼�
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

        //��{�����A�R��KEY��
        if (lastSetTimer.ContainsKey(inputPermission)) lastSetTimer.Remove(inputPermission);
        SetPermisssion(inputPermission, true);
        yield return null;
    }
    #endregion

    #region [���⪬�A]
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

        //>999f���P�ä[�]�w���A
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

        //��{�����A��j���A
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
        //���|�]���]���ഫ�a�ϦӮ���
        GameObject.DontDestroyOnLoad(this);

        //�N�ۨ��L�i���@��
        GameMaster.Instance.Player = this.gameObject;

        GameMaster.Instance.PlayerDatabase=this;

        checkSerializeField();

        //���o���O������Ƶ��c�ƥ�
        PlayerAtt = SDPlayerAtt.data;

        PlayerAtt.HorizontalAcceleration = 3;

        //�p����D�����Ѽ�
        UpdateJumpCoefficient();
    }

    //�T�{SerializeField�ŭ�
    private void checkSerializeField()
    {
        if (SDPlayerAtt == null)
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

    //��s���D�����Ѽ�
    public void UpdateJumpCoefficient()
    {
        var gravity = (-2 * PlayerAtt.JumpHeight) / (PlayerAtt.JumpToPeakTime * PlayerAtt.JumpToPeakTime);
        RB.gravityScale = Math.Abs(gravity / Physics2D.gravity.y);
        PlayerAtt.JumpStartVelocity = -gravity * (PlayerAtt.JumpToPeakTime);
    }

}
