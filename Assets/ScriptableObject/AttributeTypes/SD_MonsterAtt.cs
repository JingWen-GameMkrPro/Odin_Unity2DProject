using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SD_MonsterAtt", menuName = "ScriptableObjects/MonsterAtt", order = 2)]

public class SD_MonsterAtt : ScriptableObject
{
    [Serializable]
    public struct Data
    {
        [Header("血量")]
        public float HP;
        public float MaxHP;

        [Header("水平移動速度")]
        public float HorizontalSpeed;
        public float HorizontalAcceleration;

        [Header("水平移動速度權重")]
        public float SpeedWeight;
        public float MaxSpeedWeight;

        [Header("跳躍")]
        public float JumpHeight;
        public float JumpToPeakTime;

        [HideInInspector]
        public float JumpStartVelocity;

    }

    [SerializeField]
    public Data data = new();
}
