using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDatabase : MonoBehaviour
{
    [HideInInspector]
    public SD_MonsterAtt.Data MonsterAtt;

    //互動元件，需要調動的私人內部元件
    [SerializeField]
    public SD_MonsterAtt SD_MonsterAtt; //警告：不要修改此變數，這會導致原始資料被更動
    public Rigidbody2D RB;
    public DetecterManager DetecterManager;

    void Start()
    {
        checkSerializeField();

        //取得類別中的資料結構副本
        MonsterAtt = SD_MonsterAtt.data;
    }

    //確認SerializeField空值
    private void checkSerializeField()
    {
        if (SD_MonsterAtt == null)
        {
            Debug.LogError("SD_MonsterAtt == null");
        }

        if (RB == null)
        {
            Debug.LogError("Rigidbody2D == null");
        }

        if (DetecterManager == null)
        {
            Debug.LogError("DetecterManager == null");
        }
    }
}
