using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEditor;
using System;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class InnerInteracterMonster : MonoBehaviour 
{
    [SerializeField] float moveSpeed = 5f; // 移动速度
    [SerializeField] float attackDuration = 1f;
    [SerializeField] float damage = 10f;
    [SerializeField]
    MonsterDatabase monsterDatabase;

    [SerializeField]
    GameObject NormalAttack;

    [SerializeField]
    GameObject SkFireBall;

    [SerializeField]
    GameObject SkShockWave;

    [SerializeField]
    GameObject SkAntiShield;

    [SerializeField]
    GameObject SkBlackHole;

    [SerializeField]
    GameObject SkSkyAngry;

    bool isPlayAroundYou;
    int skill3Miss = 0;
    int HP = 100;
    int State = 0;
    bool isSkillCoolingDown = false;
    bool isSkillOK = true;

    bool isAnySkill = false;

    List<Vector3> randomWalkDirection = new()
    {
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(1, 1, 0),
        new Vector3(-1, 1, 0)
    };

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
    Dictionary<MonsterBehaviors, Coroutine> coolTimeSkills = new();
    Coroutine walkRandomTimer;
    Coroutine walkTargetTimer;

    bool isWalkTarget;
    bool isWalkRandom;

    async void decideBehavior()
    {
        if(isAnySkill)
        {
            return;
        }

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

        //如果沒有，則機率性決定行為
        var randomValue = UnityEngine.Random.Range(1, 11);
        if (randomValue <= 3)
        {
            monsterBehavior = MonsterBehaviors.WalkTarget;
        }
        else if (randomValue <= 5)
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

        if (monsterDatabase.DetecterManager.DetecterRecorderList["Detecter_NormalAttack"].Count != 0)
        {
            monsterBehavior = randomDecider((MonsterBehaviors.WalkTarget, 4), (decideSkill(), 6));
            if (monsterBehavior != MonsterBehaviors.WalkTarget)
            {
                if (walkTargetTimer != null)
                {
                    isWalkTarget = false;
                    StopCoroutine(walkTargetTimer);
                }
            }
            return;
        }

        if (!isWalkTarget)
        {
            isWalkTarget = true;
            StartCoroutine(countDownWalk(MonsterBehaviors.WalkTarget, 3));
        }

        var monsterX = transform.position;
        var playerX = GameMaster.Instance.Player.transform.position;
        var value = playerX- monsterX;
        var force = value.normalized * 10;
        monsterDatabase.RB.AddForce(force, ForceMode2D.Impulse);
        Debug.Log("doWalkTarget...");
    }

    void doWalkRandom()
    {

        if (monsterDatabase.DetecterManager.DetecterRecorderList["Detecter_NormalAttack"].Count != 0)
        {
            monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 4), (decideSkill(), 6));
            if(monsterBehavior != MonsterBehaviors.WalkRandom)
            {
                if(walkRandomTimer != null)
                {
                    isWalkRandom = false;
                    StopCoroutine(walkRandomTimer);
                }
            }
            return;
        }

        if (!isWalkRandom)
        {
            isWalkRandom = true;
            walkRandomTimer =  StartCoroutine(countDownWalk(MonsterBehaviors.WalkRandom, 1));
        }

        if(monsterDatabase.RB.velocity.magnitude == 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, randomWalkDirection.Count);
            var force = randomWalkDirection[randomIndex].normalized * 5000;
            monsterDatabase.RB.AddForce(force, ForceMode2D.Impulse);
        }

        Debug.Log("doWalkRandom...");
    }

    async Task doNormalAttack()
    {
        isAnySkill = true;
        coolDownSkill(MonsterBehaviors.NormalAttack, 3);
        await createEffect(1, 3, Vector3.zero, NormalAttack);
        isAnySkill = false;
        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));
    }

    async Task doSkFireBall()
    {
        isAnySkill = true;

        coolDownSkill(MonsterBehaviors.SkFireBall, 3);

        await createEffect(1, 3, Vector3.zero, SkFireBall);

        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));

        isAnySkill = false;
    }

    async Task doSkShockWave()
    {
        isAnySkill = true;

        coolDownSkill(MonsterBehaviors.SkShockWave, 3);
        //var LookAtQuaternion = Quaternion.LookRotation(GameMaster.Instance.Player.transform.position - transform.position);
        float eulerZ = Quaternion.Angle(transform.rotation, GameMaster.Instance.Player.transform.rotation);
        Debug.Log(GameMaster.Instance.Player.transform.position);
        Debug.Log(transform.gameObject.transform.position);
        Debug.Log(GameMaster.Instance.Player.transform.position - transform.position);

        var angle = Quaternion.Euler(0, 0, eulerZ);



        await createEffect(1, 3, Vector3.zero, SkShockWave, targetAngle: angle, parent: transform);

        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));

        isAnySkill = false;

    }

    async Task doSkAntiShield()
    {
        isAnySkill = true;

        coolDownSkill(MonsterBehaviors.SkAntiShield, 3);

        await createEffect(1, 3, new Vector3(0, 0.7f, 0), SkAntiShield, parent: transform);

        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));

        isAnySkill = false;

    }

    async Task doSkBlackHole()
    {
        isAnySkill = true;

        coolDownSkill(MonsterBehaviors.SkBlackHole, 3);

        await createEffect(1, 3, Vector3.zero, SkBlackHole);

        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));

        isAnySkill = false;

    }

    async Task createEffect(int createSeconds, float existSeconds, Vector3 position, GameObject targetEffect, Quaternion targetAngle = default, Transform parent = null)
    {
        //提示訊息
        await countDown(createSeconds);
        var effect = Instantiate(targetEffect);
 
        if (targetAngle != default)
        {
            effect.transform.rotation = targetAngle;
        }

        if(parent != null)
        {
            effect.transform.SetParent(parent);
        }
        effect.transform.position = position;
        await countDown(existSeconds);
        DestroyImmediate(effect);
    }
    
    async Task countDown(float time)
    {
        await Task.Delay((int)(time * 1000));
    }

    async Task doSkSkyAngry()
    {
        isAnySkill = true;

        coolDownSkill(MonsterBehaviors.SkSkyAngry, 3);

        await createEffect(1, 3, Vector3.zero, SkSkyAngry);

        monsterBehavior = randomDecider((MonsterBehaviors.WalkRandom, 9), (decideSkill(), 1));

        isAnySkill = false;
    }

    MonsterBehaviors randomDecider(params (MonsterBehaviors, int)[] possibleBehavior)
    {
        var sum = 0;
        foreach(var behavior in possibleBehavior)
        {
            sum += behavior.Item2;
        }
        var randomValue = UnityEngine.Random.Range(1, sum+1);
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
        //do
        //{
        //    randomChooseSkill(out chooseSkill);
        //}
        //while (coolTimeSkills.ContainsKey(chooseSkill));
        randomChooseSkill(out chooseSkill);
        if(coolTimeSkills.ContainsKey(chooseSkill))
        {
            return MonsterBehaviors.Think;
        }
        return MonsterBehaviors.SkShockWave;
    }

    void randomChooseSkill(out MonsterBehaviors chooseSkill)
    {
        chooseSkill = (MonsterBehaviors)UnityEngine.Random.Range((float)MonsterBehaviors.NormalAttack, (float)MonsterBehaviors.SkSkyAngry+1);
    }

    void coolDownSkill(MonsterBehaviors targetSkill, int continueSeconds)
    {
        Debug.Log(targetSkill + "       "+continueSeconds);
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

    void Update()
    {
        autoTurnDirection();
        decideBehavior();
    }

    void lookAtPlayerDirection()
    {
        var monsterX = transform.position.x;
        var playerX = GameMaster.Instance.Player.transform.position.x;
        var monsterLocalScale = transform.localScale;
        

        if (monsterX - playerX > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(monsterLocalScale.x), monsterLocalScale.y, monsterLocalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(monsterLocalScale.x), monsterLocalScale.y, monsterLocalScale.z);
        }
    }


    void autoTurnDirection()
    {
        var monsterLocalScale = transform.localScale;

        if (monsterDatabase.RB.velocity.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(monsterLocalScale.x), monsterLocalScale.y, monsterLocalScale.z);
        }
        else if (monsterDatabase.RB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(monsterLocalScale.x), monsterLocalScale.y, monsterLocalScale.z);
        }
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

    //void Update()
    //{
    //    decideBehavior();

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
    //}

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