using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using System;

[System.Serializable]
public class MoveHorizonal : ActionNode
{
    public enum EnumMoveDirection
    {
        Right,
        Left,
        Random
    }
    //移動方向
    public NodeProperty<EnumMoveDirection> MoveDirection;

    //移動速度(Second)
    public NodeProperty<float> MoveSpeed;

    //移動時間(Second)
    public NodeProperty<float> MoveDuration;
    
    private float moveTime = 0;
    private GameObject go;
    private Rigidbody2D rb;

    //Test
    //public BehaviourTreeInstance behaviourTreeInstance;
    protected override void OnStart() 
    {
        //Initial
        go = blackboard.Find<GameObject>("RefPlayer").value;
        rb = go.GetComponent<Rigidbody2D>();

        //異常判斷處理
        if (MoveSpeed.Value <= 0)
        {
            Debug.LogError("MoveSpeed<=0");
        }
    }

    protected override void OnStop() 
    {
        
    }

    protected override State OnUpdate() 
    {
        if (moveTime >= MoveDuration.Value)
        {
            Debug.Log("Success");
            return State.Success;
        }
        else
        {
            doTask();
        }

        return State.Running;
    }

    void doTask()
    {
        //Debug.Log("Running" + moveTime);
        //Debug.Log(message);
        //go.transform.position = Vector3.MoveTowards(go.transform.position, new Vector3(go.transform.position.x + 10, go.transform.position.y, go.transform.position.z), MoveSpeed.Value * Time.deltaTime);
        //rb.velocity = new Vector2(10 * MoveSpeed.Value, rb.velocity.y);
        moveTime = moveTime + Time.deltaTime;
    }
}
