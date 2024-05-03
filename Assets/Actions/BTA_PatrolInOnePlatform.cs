using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.Diagnostics.CodeAnalysis;

[System.Serializable]
public class BTA_PatrolInOnePlatform : ActionNode
{
    public enum PatrolDirection
    {
        Random,
        Right,
        Left
    }
    public NodeProperty<PatrolDirection> InitialDirection;
    public NodeProperty<float> HorizontalSpeed;

    private GameObject owner;
    private MonsterDatabase database;
    private PatrolDirection currentPatrolDirection;

    protected override void OnStart() 
    {
        owner = blackboard.Find<GameObject>("Owner").value;
        database = blackboard.Find<MonsterDatabase>("Database").value;
        setDirectionBySetting();
    }

    protected override void OnStop() {}

    protected override State OnUpdate() 
    {
        //持續等到落地才會開始計算邊界數據並開始移動
        if(database.RB.velocity.y==0)
        {
            doPatrol();
        }
        return State.Running;
    }

    //初始化移動方向
    private void setDirectionBySetting()
    {
        switch (currentPatrolDirection)
        {
            case PatrolDirection.Random:
                currentPatrolDirection = getRandomDirection();
                break;
            default:
                currentPatrolDirection = InitialDirection.Value;
                break;
        }
    }

    //取得隨機方向
    private PatrolDirection getRandomDirection()
    {
        int randomDirection = UnityEngine.Random.Range(0, 2);
        if (randomDirection == 0)
        {
            return PatrolDirection.Right;
        }
        else
        {
            return PatrolDirection.Left;
        }
    }

    //巡邏移動
    private void doPatrol()
    {
        if (isReachedMargin())
        {
            database.RB.velocity = new Vector2(0, database.RB.velocity.y);
            turnPatrolDirection();
        }

        database.RB.velocity = (currentPatrolDirection == PatrolDirection.Right ? new Vector2(HorizontalSpeed.Value, 0) : new Vector2(-HorizontalSpeed.Value, 0));
        maintainActorDirection();
    }

    //檢測是否到達平台邊界
    private bool isReachedMargin()
    {
        RaycastHit2D hit = Physics2D.Raycast(owner.transform.position, database.RB.velocity.x > 0 ? Vector2.right: Vector2.left, 1f, LayerMask.GetMask("MapMargin"));

        if(hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //變更巡邏方向
    private void turnPatrolDirection()
    {
        if(currentPatrolDirection == PatrolDirection.Right)
        {
            currentPatrolDirection = PatrolDirection.Left;
            Debug.Log("Turn right");

        }
        else
        {
            currentPatrolDirection = PatrolDirection.Right;
            Debug.Log("Turn left");

        }
    }

    //維護角色移動方向
    private void maintainActorDirection()
    {
        //控制方向
        if (database.RB.velocity.x > 0)
        {
            owner.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (database.RB.velocity.x < 0)
        {
            owner.transform.localScale = new Vector3(-1, 1, 1);
        }
    }


}
