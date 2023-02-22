using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableUnit : MonoBehaviour
{
    protected BeltScrollBattleManager battleManager;

    [SerializeField]
    protected HeroData heroData;
    public HeroData GetHeroData() => heroData;

    protected NavMeshAgent pathFind;

    protected AttackableUnit target;

    [SerializeField] //hp�� �ν����Ϳ��� ������ �ۼ�. 
    protected int hp;

    //��Ÿ�ӿ� ���, Hero�� Ui��ư�� ������ ��Ÿ���� �˻������� Enemy�� Boss �� ������� ������ ���� �ۼ��س���
    protected float lastNormalAttackTime;
    protected float lastPassiveSkillTime;
    protected float lastActiveSkillTime;

    protected float lastNavTime;
    protected float navDelay = 0.2f;

    //���� ������ Update�Լ� ȣ��
    protected Action nowUpdate;

    //������ ĳ���Ͱ� ����ִ� ��ų��, ���߿� ������ ��ų���� �����ϰ�, �ش� ĳ���Ͱ� ���� �̺�Ʈ�� �Ҵ����ִ� ������� ��뿹��
    protected Action NormalAttackAction;
    protected Action PassiveSkillAction;
    protected Action ActiveSkillAction;

    protected virtual void Awake()
    {
        battleManager = GameObject.FindObjectOfType<BeltScrollBattleManager>();
    }

    // AttackableHero �� AttackableEnemy ���� ������Ƽ ������
    // ���°� ��ġ�°� ����, Enemy�� ��ų�� ���� ��츦 ���
    protected UnitState unitState;
    protected virtual UnitState UnitState { get; set; }
    protected UnitBattleState battleState;
    protected virtual UnitBattleState BattleState { get; set; }

    protected bool CanNormalAttackTime {
        get {
            return (Time.time - lastNormalAttackTime) > heroData.normalAttack.cooldown;
        }
    }
    protected bool CanPassiveSkillTime {
        get {
            return (Time.time - lastPassiveSkillTime) > heroData.passiveSkill.cooldown;
        }
    }

    protected bool InRangeNormalAttack => Vector3.Distance(target.transform.position, transform.position) < heroData.normalAttack.distance;
    protected bool InRangePassiveSkill => Vector3.Distance(target.transform.position, transform.position) < heroData.passiveSkill.distance;
    protected bool NonActiveSkill => battleState != UnitBattleState.ActiveSkill && battleState != UnitBattleState.Stun;

    protected bool IsNormalAttack {
        get {
            return NonActiveSkill && InRangeNormalAttack;
        }
    }
    protected bool IsPassiveAttack {
        get {
            return NonActiveSkill && InRangePassiveSkill;
        }
    }

    protected void SetData()
    {
        pathFind.stoppingDistance = heroData.normalAttack.distance;

        //�̺�Ʈ ����. ���� �Լ����� abstract �� �����߱� ������ ��ӹ��� ĳ���͵��� ����ִ� �Լ��� ����
        NormalAttackAction = NormalAttack;
        PassiveSkillAction = PassiveSkill;
        ActiveSkillAction = ActiveAttack;

        hp = heroData.stats.healthPoint; //���� �ܿ� Hp�����Ͱ� ���⿡ HeroData�� �ִ� �ִ�ü�� ���
    }
    protected void FixedUpdate()
    {
        nowUpdate?.Invoke();
    }

    //��Ÿ�� �нú� ��ų �ǽð����� ���

    //Enemy�� Hero�� �������� ����ִ� �Լ�. Hero�� SetMoveNext�� SetRetrunPos �� �� ������ ����
    public abstract void SetBattle();

    //�ش��ϴ� ��ų���� ��ӹ��� ĳ���͵��� ������ �־�� ��
    //�ش��ϴ� ��ų�� �������� ���� NormalAttackAction �� �Ҵ��� �Լ��� �ٲ��ְ����
    public abstract void NormalAttack();
    public abstract void PassiveSkill();
    public abstract void ActiveAttack();

    //���� �ִϸ��̼��� ������, ���¸� NormalAttack�� �ٲ��� �ӽ� �Լ�
    public abstract void TestPassiveEnd();
    public abstract void TestActiveEnd();

    //ĳ���͵��� ���¸����� ������Ʈ �Լ�. Enemy�� ������� �ʴ� ���¿�, Update�� ������ ����. �ƹ����X
    protected abstract void IdleUpdate();
    protected abstract void BattleUpdate();
    protected abstract void DieUpdate();
    protected abstract void MoveNextUpdate();
    protected abstract void ReturnPosUpdate();

    public abstract void OnDamage(int dmg);
}
