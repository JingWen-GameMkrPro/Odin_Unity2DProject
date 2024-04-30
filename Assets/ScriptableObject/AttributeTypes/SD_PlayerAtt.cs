using System;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "SD_PlayerAtt", menuName = "ScriptableObjects/PlayerAtt", order = 1)]
public class SD_PlayerAtt : ScriptableObject
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

    public Data data = new();
}
