using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.AI;

public class AttackableHero : MonoBehaviour
{
    protected CharactorData charactorData;
    protected BattleHero heroUi;
    private NavMeshAgent pathFind;

    [SerializeField]
    public GameObject target;

    [SerializeField]
    private float coolDownTimer;
    private float coolDown;

    private Vector3 returnPos = Vector3.zero;

    public Action nowUpdate;
    public Action NormalSkill;

    HeroState heroState;
    public HeroState HeroState {
        get {
            return heroState;
        }
        set {
            if (heroState == value)
                return;

            heroState = value;
            switch (heroState)
            {
                case HeroState.None:
                    break;
                case HeroState.Idle:
                    pathFind.isStopped = true;
                    nowUpdate = IdleUpdate;
                    break;
                case HeroState.ReturnPosition: // ���ġ
                    pathFind.isStopped = false;
                    pathFind.stoppingDistance = 0; //������ ����
                    nowUpdate = ReturnPosUpdate;
                    break;
                case HeroState.MoveNext:
                    pathFind.isStopped = false;
                    nowUpdate = ReturnPosUpdate;
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
                case HeroState.Count:
                    break;
                default:
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

    private void Awake()
    {
        charactorData = LoadTestData(); //�ӽ� ������ �ε�
        pathFind = transform.GetComponent<NavMeshAgent>();
        SetData();
    }

    // Ui�� ����, Ui�� ��ų ��Ÿ�� ����
    public virtual void SetUi(BattleHero heroUI)
    {
        this.heroUi = heroUI;
        this.heroUi.heroSkill.Set(charactorData.skillCooldown, Skill); //�ñر� ��Ÿ�Ӱ� �ñر� �Լ� ���
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
        Logger.Debug("Skill");
    }

    public CharactorData LoadTestData()
    {
        CharactorData testData = new CharactorData();

        testData.damage = 100;
        testData.def = 20;
        testData.hp = 100;
        testData.speed = 20;
        testData.chritical = 50;
        testData.chriticalDmg = 2;
        testData.evasion = 20;
        testData.accuracy = 80;

        testData.attackDistance = 3f;
        testData.attackCount = 1;

        testData.grade = 'A';
        testData.level = 1;
        testData.type = "TEST";
        testData.cooldown = 1;
        testData.skillCooldown = 0.3f;
        testData.exp = 0;

        return testData;
    }

    public void IdleUpdate()
    {

    }

    public void ReturnPosUpdate()
    {
        pathFind.SetDestination(returnPos); //���ġ ��ġ ����
    }

    public void BattleUpdate()
    {
        switch (HeroBattleState)
        {
            case HeroBattleState.None:
                break;
            case HeroBattleState.Normal:
                //Ÿ���� ������ Ÿ�� ��ô
                if (target == null)
                {
                    SetTarget();
                    return;
                }

                //Ÿ������ �̵�
                pathFind.SetDestination(target.transform.position);

                //Ÿ�ٰ� ���� ���� �ȿ� ������, �Ϲݽ�ų �����̰�, ��Ÿ�� ������ �����ɶ�
                if (IsAttack && IsCoolDown)
                {
                    coolDownTimer = coolDown;
                    NormalSkill();
                }
                break;
            case HeroBattleState.Ulitimate:
                break;
            case HeroBattleState.Stun:
                break;
            case HeroBattleState.Count:
                break;
            default:
                break;
        }

    }

    public void DieUpdate()
    {

    }
}
