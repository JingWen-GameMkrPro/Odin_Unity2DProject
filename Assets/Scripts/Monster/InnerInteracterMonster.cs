using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEditor;
using System;
using Unity.Mathematics;

public class InnerInteracterMonster : MonoBehaviour 
{
    [SerializeField] float moveSpeed = 5f; // 移动速度
    [SerializeField] float attackDuration = 1f;
    [SerializeField] float damage = 10f;
    bool isPlayAroundYou;
    int skill3Miss = 0;
    int HP = 100;
    int State = 0;
    bool isSkillCoolingDown = false;
    bool isSkillOK = true;

    enum monsterBehavior
    {
        Idle,
        WalkTarget,
        WalkRandom,
        NormalAttack,
        skFireBall,
        skShockWave,
        skAntiShield,
        skBlackHole,
        skSkyAngry
    }



    //怪物在每一次更新時，狀態一定只會有一個，不會同時施放技能一又放技能二，因此只需要有唯一的狀態變數

    void decideState()
    {
        State = 0;

    }

    void waitCoolTime(ref bool state, int time)
    {
        state = false;
        //開始計時，call 計時器，如何確保Timer執行完了
        //StartCoroutine(Timer(time));
        state = true;

    }

    //傳值、傳址
    IEnumerator Timer(int time)
    {

        yield return new WaitForSeconds(1);
        time = time - 1;
        if(time < 0)
        {
            Debug.Log("In Timer");
        }
    }
    private void Start()
    {
        waitCoolTime(ref isSkillOK, 5);
    }

    void Update()
    {
        if (isSkillCoolingDown)
        {
            return;
        }
        
        if (isPlayAroundYou) 
        {
            StartCoroutine(AttackWithDelay());
        }
        else 
        {
            MoveToPlayer();
        }

        if (Input.GetKeyDown(KeyCode.N)) // 檢查用程式碼
        {
            HP = 30;
            Debug.Log(HP);
        }
        if (Input.GetKeyDown(KeyCode.M)) // 檢查用程式碼
        {
            HP = 100;
            Debug.Log(HP);
        }

        switch(State) // 技能選擇器，順序依觸發難度選擇
        {
            case 0:
                StartCoroutine(WaitForSkillet5Ask());
                break;
            case 1:
                StartCoroutine(WaitForSkillet4Ask());
                break;   
            case 2:
                StartCoroutine(WaitForSkillet3Ask());
                break;
            case 3:
                StartCoroutine(WaitForSkillet2Ask());
                break;
            case 4:
                decideState();
                break;
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
        GameMaster.Instance.OuterInteracterPlayer.ReduceHP(this.gameObject, damage);
        //GameMaster.Instance.PlayerDatabase.PlayerAtt.HP -= damage;
        // 能夠造成傷害，但碰撞一次觸發過多次的扣血
        //Debug.Log("Attacking");
    }

    void Skill2() 
    {
        // 執行技能
    }

    IEnumerator WaitForSkillet2Ask()
    {
        Vector3 currentPosition = transform.position;
        var targetPosition = GameMaster.Instance.Player.transform.position;
        if (Math.Abs(currentPosition.y - targetPosition.y) < 2) // 判斷是否攻擊
        {
            isSkillCoolingDown = false;
            Skill2();
            //Debug.Log("Use Skill2");
            decideState();
        }
        else // 執行下一個技能，避免卡死
        {
            State += 1;
        }

        yield return new WaitForSeconds(attackDuration);
        // anim
        isSkillCoolingDown = true;
    }

    void Skill3() 
    {
        // 使用技能
    }

    IEnumerator WaitForSkillet3Ask()
    {
        Vector3 currentPosition = transform.position;
        var targetPosition = GameMaster.Instance.Player.transform.position;
        float distance = Vector3.Distance(currentPosition, targetPosition);
        // float distance = (float)Math.Pow( Math.Pow((currentPosition.x - targetPosition.x), 2) + Math.Pow((currentPosition.y - targetPosition.y), 2), 1/2);

        if (distance > 8f) 
        {
            isSkillCoolingDown = true;
            Skill3();
            //Debug.Log("Use Skill3");
            decideState();
        }   
        else 
        {
            State += 1;
            skill3Miss += 1;
        }

        yield return new WaitForSeconds(attackDuration);

        isSkillCoolingDown = false;
    }

    void Skill4() 
    {
        // 使用技能
       
    }

    IEnumerator WaitForSkillet4Ask()
    {
        if (skill3Miss >= 3)
        {
            isSkillCoolingDown = true;
            Skill4();
            //Debug.Log("Use Skill4");
            decideState();
            skill3Miss = 0;
        }
        else
        {
            State += 1;
        }
        

        yield return new WaitForSeconds(attackDuration);
        isSkillCoolingDown = false;
    }

    void Skill5() 
    {
        // 使用技能
    }

    IEnumerator WaitForSkillet5Ask()
    {
        if (HP <= 30)
        {
            isSkillCoolingDown = true;
            Skill5();
            //Debug.Log("Use Skill5");
            decideState();
        }
        else
        {
            State += 1;
        }
        
        yield return new WaitForSeconds(attackDuration);
        isSkillCoolingDown = false;
    }

    IEnumerator AttackWithDelay()
    {
        isSkillCoolingDown = true;
        // 调用攻击方法
        Attack();

        // 等待攻击动画播放完成
        yield return new WaitForSeconds(attackDuration);

        // 攻击动画播放完成后，将Attacking设置为false
        GetComponent<Animator>().SetBool("BossAttack", false);
        isPlayAroundYou = false;
        //Debug.Log("Attack finished");
        isSkillCoolingDown = false;
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player") 
        {
            isPlayAroundYou = true;
        }
    }
}