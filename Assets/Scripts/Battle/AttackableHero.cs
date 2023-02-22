using UnityEngine;
using UnityEngine.AI;

public class AttackableHero : AttackableUnit
{
    protected BattleHero heroUI;

    [SerializeField]
    private Transform returnPos;
    public void SetReturnPos(Transform tr) => returnPos = returnPos = tr; 
    public float skillDuration; // �ӽ� ����

    public UnitState UnitState {
        get {
            return unitState;
        }
        set {
            if (unitState == value)
                return;

            unitState = value;
            heroUI.heroState = unitState;
            switch (unitState)
            {
                case UnitState.Idle:
                    pathFind.isStopped = true;
                    nowUpdate = IdleUpdate;
                    break;
                case UnitState.ReturnPosition: // ���ġ
                    pathFind.isStopped = false;
                    pathFind.speed = 10;
                    pathFind.SetDestination(returnPos.position); //���ġ ��ġ ����
                    pathFind.stoppingDistance = 0; //������ ����
                    nowUpdate = ReturnPosUpdate;
                    break;
                case UnitState.MoveNext:
                    pathFind.isStopped = false;
                    nowUpdate = MoveNextUpdate;
                    break;
                case UnitState.Battle:
                    pathFind.speed = heroData.stats.moveSpeed;
                    pathFind.isStopped = false;
                    HeroBattleState = UnitBattleState.Common;
                    nowUpdate = BattleUpdate;
                    break;
                case UnitState.Die:
                    pathFind.isStopped = true;
                    nowUpdate = DieUpdate;
                    Destroy(gameObject, 1);
                    break;
            }
        }
    }

    UnitBattleState heroBattleState;
    public UnitBattleState HeroBattleState {
        get {
            return heroBattleState;
        }
        set {
            if (value == heroBattleState)
                return;
            heroBattleState = value;
        }
    }

    protected override void Awake()
    {
        //charactorData = LoadTestData(); //�ӽ� ������ �ε�
        pathFind = transform.GetComponent<NavMeshAgent>();
        SetData();

        unitState = UnitState.Idle;
    }

    // Ui�� ����, Ui�� ��ų ��Ÿ�� ����
    public virtual void SetUi(BattleHero _heroUI)
    {
        heroUI = _heroUI;
        heroUI.heroSkill.Set(heroData.activeSkill.cooldown, AutoAttack); //�ñر� ��Ÿ�Ӱ� �ñر� �Լ� ���
    }

    public override void CommonAttack()
    {
    }
    public override void AutoAttack()
    {

    }
    public override void ActiveAttack()
    {
        activeStartTime = Time.time;
        BattleState = UnitBattleState.Action;
        Logger.Debug("Skill");
    }

    protected override void IdleUpdate()
    {

    }
    protected override void BattleUpdate()
    {
        switch (HeroBattleState)
        {
            case UnitBattleState.Common:
                //Ÿ���� ������ Ÿ�� ��ô
                if (target == null)
                {
                    SetTarget();
                    return;
                }

                //Ÿ������ �̵�
                pathFind.SetDestination(target.transform.position);

                //Ÿ�ٰ� ���� ���� �ȿ� ������, �Ϲݽ�ų �����̰�, ��Ÿ�� ������ �����ɶ�
                if (IsAttack && CanNormalAttack)
                {
                    lastNormalAttackTime = Time.time;
                    Common();
                }
                break;
            case UnitBattleState.Action:
                if (Time.time - activeStartTime > skillDuration)
                {
                    HeroBattleState = UnitBattleState.Common;
                }
                break;
            case UnitBattleState.Stun:
                break;
        }

    }

    protected override void DieUpdate()
    {

    }

    protected override void MoveNextUpdate()
    {

    }

    protected override void ReturnPosUpdate()
    {
        switch(pathFind.isStopped)
        {
            case true:
                transform.rotation = Quaternion.Lerp(transform.rotation, returnPos.rotation, Time.deltaTime * 5);

                float angle = Quaternion.Angle(transform.rotation, returnPos.rotation);

                if (angle <= 0)
                {
                    
                    UnitState = UnitState.Idle;
                }
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

    [ContextMenu("Battle")]
    protected override void SetTestBattle()
    {
        UnitState = UnitState.Battle;
    }

    public override void OnDamage(int dmg)
    {
        hp = Mathf.Max(hp - dmg, 0);
        if (hp <= 0)
            UnitState = UnitState.Die;
    }
}