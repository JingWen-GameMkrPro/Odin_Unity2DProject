using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEditor;

public class InnerInteracterMonster : MonoBehaviour 
{
    [SerializeField] float moveSpeed = 5f; // 移动速度
    [SerializeField] float attackDuration = 1f;
    [SerializeField] float damage = 10f;
    bool isPlayAroundYou;

    // 每帧都调用
    void Update()
    {
        if (isPlayAroundYou)
        {
            StartCoroutine(AttackWithDelay());
        }
        else
        {
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        // 使魔王物件移動向玩家
        // var owner = blackboard.Find<GameObject>("Owner").value;
        Vector3 currentPosition = transform.position;
        var targetPosition = GameMaster.Instance.Player.transform.position;
        
        // 平滑移动
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
        if (targetPosition.x - currentPosition.x < 0) 
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else 
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        // Debug.Log("Moving");
    }

    void Attack()
    {
        GetComponent<Animator>().SetBool("BossAttack", true);
        GameMaster.Instance.PlayerDatabase.PlayerAtt.HP -= damage;
        // 能夠造成傷害，但碰撞一次觸發過多次的扣血
        Debug.Log("Attacking");
    }

    IEnumerator AttackWithDelay()
    {
        // 调用攻击方法
        Attack();

        // 等待攻击动画播放完成
        yield return new WaitForSeconds(attackDuration);

        // 攻击动画播放完成后，将Attacking设置为false
        GetComponent<Animator>().SetBool("BossAttack", false);
        isPlayAroundYou = false;
        Debug.Log("Attack finished");
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player") 
        {
            isPlayAroundYou = true;
        }
    }
}
