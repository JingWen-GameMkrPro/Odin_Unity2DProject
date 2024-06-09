using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // 在碰撞開始時被調用
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 檢查碰撞的對象是否是小怪
        if (collision.gameObject.CompareTag("Monster"))
        {
            // 在控制台中輸出碰撞事件
            Debug.Log("Player collided with monster!");

            // 在這裡可以執行與碰撞相關的操作，例如對玩家造成傷害、播放音效等等
        }
    }
}
