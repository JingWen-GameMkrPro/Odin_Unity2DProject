using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterInteracterMonster : OuterInteracterBase
{
    [SerializeField]
    public MonsterDatabase MonsterDatabase;

    private void Start()
    {
        checkSerializeField();
    }

    //½T»{SerializeFieldªÅ­È
    private void checkSerializeField()
    {
        if (MonsterDatabase == null)
        {
            Debug.LogError("MonsterDatabase == null");
        }
    }

    public override void ReduceHP(GameObject causer, float value)
    {
        if(!MonsterDatabase.isAntiMode)
        {
            MonsterDatabase.MonsterAtt.HP = Mathf.Clamp(MonsterDatabase.MonsterAtt.HP - value, 0, MonsterDatabase.MonsterAtt.MaxHP);
            MonsterDatabase.BeDamagedRecordValue = MonsterDatabase.BeDamagedRecordValue + (int)value;
        }
        else
        {

        }
    }

    public int GetBeDamagedRecordValue()
    {
        return MonsterDatabase.BeDamagedRecordValue;
    }
}
