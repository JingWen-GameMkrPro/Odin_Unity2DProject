using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 玩家生命系統
    /// </summary>
    private HealthSystem _healthSystem;

    void Awake()
    {
        // 嘗試獲取 HealthSystem 組件
        _healthSystem = GetComponent<HealthSystem>();

        // 如果 _healthSystem 為 null，輸出錯誤訊息
        if (_healthSystem == null)
        {
            Debug.LogError("HealthSystem component not found on the player object.");
        }
    }

    // 當玩家與其他對象發生碰撞時調用
    void OnCollisionEnter(Collision collision)
    {
        // 檢查碰撞的對象是否是小怪（假設小怪有一個標籤 "Enemy"）
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (_healthSystem != null)
            {
                // 對玩家造成傷害，這裡假設小怪每次碰撞造成 1 點傷害
                _healthSystem.Damage(1);
            }
        }
    }
}
