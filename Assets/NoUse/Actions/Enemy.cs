using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 在碰撞開始時被調用
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 檢查碰撞的對象是否是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            // 在控制台中輸出碰撞事件
            Debug.Log("Enemy collided with player!");

            // 如果玩家身上有 HealthSystem 腳本，對其造成傷害
            HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                // 對玩家造成傷害
                playerHealth.Damage(1);
            }
        }
    }
}
