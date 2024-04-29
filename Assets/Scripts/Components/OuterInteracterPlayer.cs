using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OuterInteracterPlayer : OuterInteracterBase
{
    [SerializeField]
    PlayerDatabase PlayerDatabase;

    private void Start()
    {
        checkSerializeField();
    }

    //½T»{SerializeFieldªÅ­È
    private void checkSerializeField()
    {
        if (PlayerDatabase == null)
        {
            Debug.LogError("PlayerDatabase == null");
        }
    }

    public override void ReduceHP(GameObject causer, float value)
    {
        PlayerDatabase.PlayerAtt.HP = Mathf.Clamp(PlayerDatabase.PlayerAtt.HP - value, 0, PlayerDatabase.PlayerAtt.MaxHP);
    }


}
