using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEditor;
using System;

public class InnerInteracterMonster : MonoBehaviour 
{
    [SerializeField] float moveSpeed = 5f; // 移动速度
    [SerializeField] float attackDuration = 1f;
    [SerializeField] float damage = 10f;
    [SerializeField]
    MonsterDatabase monsterDatabase;

    bool isPlayAroundYou;
    int skill3Miss = 0;
    int HP = 100;
    int State = 0;
    bool isSkillCoolingDown = false;
    bool isSkillOK = true;

    public enum MonsterBehaviors
    {
        Think = -1,
        Idle = 0,
        WalkTarget = 1,
        WalkRandom = 2,
        NormalAttack = 3,
        SkFireBall = 4,
        SkShockWave = 5,
        SkAntiShield = 6,
        SkBlackHole = 7,
        SkSkyAngry = 8
    }
    MonsterBehaviors monsterBehavior = MonsterBehaviors.Think;
    //如果技能進入冷卻時間，則字典會新增該技能直到冷卻結束
    private Dictionary<MonsterBehaviors, Coroutine> coolTimeSkills = new();

    bool isWalkTarget;
    bool isWalkRandom;

    void decideBehavior()
    {
        //攻擊後，一定會回到Idle，再重新根據冷卻狀況判斷其他行為
        switch (monsterBehavior)
        {
            case MonsterBehaviors.Idle:
                break;
            case MonsterBehaviors.Think:
                doThink();
                break;
            case MonsterBehaviors.WalkTarget:
                doWalkTarget();
                break;
            case MonsterBehaviors.WalkRandom:
                doWalkRandom();
                break;
            case MonsterBehaviors.NormalAttack:
                doNormalAttack();
                break;
            case MonsterBehaviors.SkFireBall:
                doSkFireBall();
                break;
            case MonsterBehaviors.SkShockWave:
                doSkShockWave();
                break;
            case MonsterBehaviors.SkAntiShield:
                doSkAntiShield();
                break;
            case MonsterBehaviors.SkBlackHole:
                doSkBlackHole();
                break;
            case MonsterBehaviors.SkSkyAngry:
                doSkSkyAngry();
                break;
        }
    }
    void doThink()
    {
        Debug.LogWarning("Think");
        //如果範圍內有敵人、則一定會攻擊
        //if (monsterDatabase.DetecterManager.DetecterRecorderList["Detecter_NormalAttack"].Count != 0)
        //{
        //    monsterBehavior = MonsterBehaviors.NormalAttack;
        //    return;
        //}

        //如果沒有，則機率性決定行為
        var randomValue = UnityEngine.Random.Range(0, 10);
        if (randomValue <= 3)
        {
            monsterBehavior = MonsterBehaviors.WalkTarget;
        }
        else if (randomValue <= 6)
        {
            monsterBehavior = MonsterBehaviors.WalkRandom;
        }
        else if (randomValue <= 10)
        {
            monsterBehavior = decideSkill();
        }
    }



    void doWalkTarget()
    {
        if(monsterDatabase.DetecterManager.DetecterRecorderList["Detecter_NormalAttack"].Count != 0)
        {
            monsterBehavior = MonsterBehaviors.Think;
            return;
        }

        if (!isWalkTarget)
        {
            isWalkTarget = true;
            StartCoroutine(countDownWalk(MonsterBehaviors.WalkTarget, 3));
        }
        Debug.Log("doWalkTarget...");
    }

    void doWalkRandom()
    {
        if (!isWalkRandom)
        {
            isWalkRandom = true;
            StartCoroutine(countDownWalk(MonsterBehaviors.WalkRandom, 3));
        }
        Debug.Log("doWalkRandom...");
    }

    void doNormalAttack()
    {
        Debug.Log("NormalAttack");
        coolDownSkill(MonsterBehaviors.NormalAttack, 3);
        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 7), (decideSkill(), 3));
    }

    void doSkFireBall()
    {
        Debug.Log("SkFireBall");
        coolDownSkill(MonsterBehaviors.SkFireBall, 3);
        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));


    }

    void doSkShockWave()
    {
        Debug.Log("SkShockWave");
        coolDownSkill(MonsterBehaviors.SkShockWave, 3);
        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));
    }

    void doSkAntiShield()
    {
        Debug.Log("SkAntiShield");
        coolDownSkill(MonsterBehaviors.SkAntiShield, 3);
        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));
    }

    void doSkBlackHole()
    {
        Debug.Log("SkBlackHole");
        coolDownSkill(MonsterBehaviors.SkBlackHole, 3);
        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));




    }

    void doSkSkyAngry()
    {
        Debug.Log("SkSkyAngry");
        coolDownSkill(MonsterBehaviors.SkSkyAngry, 3);
        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));



    }

    MonsterBehaviors randomDecider(params (MonsterBehaviors, int)[] possibleBehavior)
    {
        var sum = 0;
        foreach(var behavior in possibleBehavior)
        {
            sum += behavior.Item2;
        }
        var randomValue = UnityEngine.Random.Range(0, sum);
        int value = 0;
        foreach (var behavior in possibleBehavior)
        {
            value += behavior.Item2;
            if(randomValue <= value)
            {
                return behavior.Item1;
            }
        }
        return MonsterBehaviors.Think;

    }

    MonsterBehaviors decideSkill()
    {
        //目前全部技能滿了，則回到idle狀態'
        if(coolTimeSkills.Count == 6)
        {
            return MonsterBehaviors.Think;
        }

        //隨機決定一個技能
        MonsterBehaviors chooseSkill;
        do
        {
            randomChooseSkill(out chooseSkill);
        }
        while (coolTimeSkills.ContainsKey(chooseSkill));
        return chooseSkill;
    }

    void randomChooseSkill(out MonsterBehaviors chooseSkill)
    {
        chooseSkill = (MonsterBehaviors)UnityEngine.Random.Range((float)MonsterBehaviors.NormalAttack - 1, (float)MonsterBehaviors.SkSkyAngry);
    }

    void coolDownSkill(MonsterBehaviors targetSkill, int continueSeconds)
    {
        //如果還沒被封鎖
        if (!coolTimeSkills.ContainsKey(targetSkill))
        {
            //新增倒數
            coolTimeSkills.Add(targetSkill, StartCoroutine(countDownCoolTime(targetSkill, continueSeconds)));
        }
        //如果正在被封鎖
        else
        {
            //刷新倒數
            StopCoroutine(coolTimeSkills[targetSkill]);
            coolTimeSkills[targetSkill] = StartCoroutine(countDownCoolTime(targetSkill, continueSeconds));
        }
    }

    IEnumerator countDownCoolTime(MonsterBehaviors coolDownSkill, int inputSeconds = 1)
    {
        int currentSeconds = inputSeconds;
        while (currentSeconds > 0)
        {
            yield return new WaitForSeconds(1);
            currentSeconds--;
        }

        //協程結束，刪除KEY值
        if (coolTimeSkills.ContainsKey(coolDownSkill)) coolTimeSkills.Remove(coolDownSkill);
        yield return null;
    }

    IEnumerator countDownWalk(MonsterBehaviors type, int inputSeconds = 1)
    {
        Debug.Log(type);
        yield return new WaitForSeconds(inputSeconds);
        monsterBehavior = MonsterBehaviors.Think;
        if(type == MonsterBehaviors.WalkTarget)
        {
            isWalkTarget = false;
        }
        else if ((type == MonsterBehaviors.WalkRandom))
        {
            isWalkRandom = false;
        }
        Debug.Log("WalkEnd");
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
        //waitCoolTime(ref isSkillOK, 5);
    }

    void Update()
    {
        decideBehavior();

        //if (isSkillCoolingDown)
        //{
        //    return;
        //}

        //if (isPlayAroundYou) 
        //{
        //    StartCoroutine(AttackWithDelay());
        //}
        //else 
        //{
        //    MoveToPlayer();
        //}

        //if (Input.GetKeyDown(KeyCode.N)) // 檢查用程式碼
        //{
        //    HP = 30;
        //    Debug.Log(HP);
        //}
        //if (Input.GetKeyDown(KeyCode.M)) // 檢查用程式碼
        //{
        //    HP = 100;
        //    Debug.Log(HP);
        //}

        //switch(State) // 技能選擇器，順序依觸發難度選擇
        //{
        //    case 0:
        //        StartCoroutine(WaitForSkillet5Ask());
        //        break;
        //    case 1:
        //        StartCoroutine(WaitForSkillet4Ask());
        //        break;   
        //    case 2:
        //        StartCoroutine(WaitForSkillet3Ask());
        //        break;
        //    case 3:
        //        StartCoroutine(WaitForSkillet2Ask());
        //        break;
        //    case 4:
        //        decideState();
        //        break;
        //}
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