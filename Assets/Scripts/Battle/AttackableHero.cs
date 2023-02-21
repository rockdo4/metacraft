using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AttackableHero : MonoBehaviour
{
    protected CharacterData charactorData;
    protected BattleHero heroUi;
    private NavMeshAgent pathFind;

    [SerializeField]
    public GameObject target;

    [SerializeField]
    private float coolDownTimer;
    private float coolDown;

    [SerializeField]
    private Transform returnPos;

    public Action nowUpdate;
    public Action NormalSkill;

    float ultimateStartTime;

    HeroState heroState;
    public HeroState HeroState {
        get {
            return heroState;
        }
        set {
            if (heroState == value)
                return;

            heroState = value;
            heroUi.heroState = heroState;
            switch (heroState)
            {
                case HeroState.Idle:
                    pathFind.isStopped = true;
                    nowUpdate = IdleUpdate;
                    break;
                case HeroState.ReturnPosition: // 재배치
                    pathFind.isStopped = false;
                    pathFind.SetDestination(returnPos.position); //재배치 위치 설정
                    pathFind.stoppingDistance = 0; //가까이 가기
                    nowUpdate = ReturnPosUpdate;
                    break;
                case HeroState.MoveNext:
                    pathFind.isStopped = false;
                    nowUpdate = MoveNextUpdate;
                    break;
                case HeroState.Battle:
                    pathFind.isStopped = false;
                    HeroBattleState = HeroBattleState.Normal;
                    nowUpdate = BattleUpdate;
                    break;
                case HeroState.Die:
                    pathFind.isStopped = true;
                    nowUpdate = DieUpdate;
                    break;
            }
        }
    }

    HeroBattleState heroBattleState;
    public HeroBattleState HeroBattleState {
        get {
            return heroBattleState;
        }
        set {
            if (value == heroBattleState)
                return;
            heroBattleState = value;
        }
    }

    public bool IsCoolDown {
        get {
            return coolDownTimer <= 0;
        }
    }

    public bool InRange => Vector3.Distance(target.transform.position, transform.position) < charactorData.attackDistance;
    public bool IsNormal => heroBattleState != HeroBattleState.Ulitimate && heroBattleState != HeroBattleState.Stun;

    public bool IsAttack {
        get {
            return IsNormal && InRange;
        }
    }

    Coroutine ReadyRotate;

    private void Awake()
    {
        charactorData = LoadTestData(); //임시 데이터 로드
        pathFind = transform.GetComponent<NavMeshAgent>();
        SetData();

        HeroState = HeroState.Idle;
    }

    // Ui와 연결, Ui에 스킬 쿨타임 연결
    public virtual void SetUi(BattleHero heroUI)
    {
        this.heroUi = heroUI;
        this.heroUi.heroSkill.Set(charactorData.skillCooldown, Skill); //궁극기 쿨타임과 궁극기 함수 등록
    }
    private void SetData()
    {
        coolDown = charactorData.cooldown;
        pathFind.stoppingDistance = charactorData.attackDistance;
        NormalSkill = Attack;
    }

    private void FixedUpdate()
    {
        CoolDownUpdate();
        if(nowUpdate != null)
            nowUpdate();
    }

    public void CoolDownUpdate()
    {
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
            coolDownTimer = Mathf.Max(0, coolDownTimer);
        }
    }

    private void SetTarget()
    {
        var Enemys = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        if (Enemys.Count == 0)
        {
            target = null;
            HeroState = HeroState.ReturnPosition;
            return;
        }

        target = Enemys.OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
                          .FirstOrDefault();
    }

    [ContextMenu("Walk")]
    public void SetTestWalk()
    {
        HeroState = HeroState.Battle;
    }

    public virtual void Attack()
    {
        Logger.Debug("Attack");
    }
    public virtual void Skill()
    {
        ultimateStartTime = Time.time;
        HeroBattleState = HeroBattleState.Ulitimate;
        Logger.Debug("Skill");
    }

    public CharacterData LoadTestData()
    {
        CharacterData testData = new ();

        testData.damage = 100;
        testData.def = 20;
        testData.hp = 100;
        testData.speed = 20;
        testData.critical = 50;
        testData.criticalDmg = 2;
        testData.evasion = 20;
        testData.accuracy = 80;

        testData.attackDistance = 3f;
        testData.attackCount = 2;

        testData.grade = "A";
        testData.level = 1;
        testData.job = "TEST";
        testData.cooldown = 1;
        testData.skillCooldown = 0.3f;
        testData.skillDuration = 3f;
        testData.exp = 0;

        return testData;
    }

    private void IdleUpdate()
    {

    }
    Vector3 test;
    private void ReturnPosUpdate()
    {
        switch(pathFind.isStopped)
        {
            case true:
                transform.rotation = Quaternion.Lerp(transform.rotation, returnPos.rotation, Time.deltaTime * 5);


                break;
            case false:
                if (Vector3.Distance(returnPos.position, transform.position) < 0.1f)
                {
                    pathFind.isStopped = true;
                    transform.position = returnPos.position;
                }
                break;
        }
    }


    private void MoveNextUpdate()
    {

    }

    private void BattleUpdate()
    {
        switch (HeroBattleState)
        {
            case HeroBattleState.Normal:
                //타겟이 없으면 타겟 추척
                if (target == null)
                {
                    SetTarget();
                    return;
                }

                //타겟으로 이동
                pathFind.SetDestination(target.transform.position);

                //타겟과 일정 범위 안에 있으며, 일반스킬 상태이고, 쿨타임 조건이 충족될때
                if (IsAttack && IsCoolDown)
                {
                    coolDownTimer = coolDown;
                    NormalSkill();
                }
                break;
            case HeroBattleState.Ulitimate:
                if(Time.time - ultimateStartTime > charactorData.skillDuration)
                {
                    HeroBattleState = HeroBattleState.Normal;
                }
                break;
            case HeroBattleState.Stun:
                break;
        }

    }

    private void DieUpdate()
    {

    }
}