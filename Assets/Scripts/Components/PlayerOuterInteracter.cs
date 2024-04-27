using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerOuterInteracter : MonoBehaviour
{
    [SerializeField]
    public PlayerBehavior PlayerBehavior;

    private void Start()
    {
        checkSerializeField();
    }

    //確認SerializeField空值
    void checkSerializeField()
    {
        if (PlayerBehavior == null)
        {
            Debug.LogError("PlayerBehavior == null");
        }
    }

    //信號：做出被扣血行為
    public void NotifyReduceHP(float reduceValue)
    {
        PlayerBehavior.DoReduceHP(reduceValue);
    }

    //信號：做出扣人血行為
    public void NotifyApplyDamage(float reduceValue)
    {
        PlayerBehavior.DoApplyDamage(reduceValue);
    }
}
