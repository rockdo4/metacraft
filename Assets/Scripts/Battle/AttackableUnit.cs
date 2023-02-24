using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackableUnit : MonoBehaviour
{
    protected BeltScrollBattleManager battleManager;

    [SerializeField]
    protected CharacterDataBundle characterData;
    public CharacterDataBundle GetHeroData() => characterData;

    protected NavMeshAgent pathFind;

    protected AttackableUnit target;
    [SerializeField]
    protected List<AttackableHero> heroList;
    [SerializeField]
    protected List<AttackableEnemy> enemyList;

    [SerializeField]
    protected int hp;
    public int GetHp() => hp;

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

    //Ÿ�� ã���� true Ÿ�ٿ��� �����ϸ� false
    protected bool moveTarget;

    protected Animator animator;

    protected virtual void Awake()
    {
        battleManager = FindObjectOfType<BeltScrollBattleManager>();
        animator = GetComponent<Animator>();
    }

    // AttackableHero �� AttackableEnemy ���� ������Ƽ ������
    // ���°� ��ġ�°� ����, Enemy�� ��ų�� ���� ��츦 ���
    [SerializeField]
    protected UnitState unitState;
    protected virtual UnitState UnitState { get; set; }

    [SerializeField]
    protected UnitBattleState battleState;
    protected virtual UnitBattleState BattleState { get; set; }

    protected bool CanNormalAttackTime {
        get {
            return (Time.time - lastNormalAttackTime) > characterData.attack.cooldown;
        }
    }
    protected bool CanPassiveSkillTime {
        get {
            return (Time.time - lastPassiveSkillTime) > characterData.passiveSkill.cooldown;
        }
    }

    protected bool InRangeNormalAttack => Vector3.Distance(target.transform.position, transform.position) < characterData.attack.distance;
    protected bool InRangePassiveSkill => Vector3.Distance(target.transform.position, transform.position) < characterData.passiveSkill.distance;
    protected bool NonActiveSkill => battleState != UnitBattleState.ActiveSkill && battleState != UnitBattleState.Stun;

    protected void SetData()
    {
        pathFind.stoppingDistance = characterData.attack.distance * 0.9f;

        //�̺�Ʈ ����. ���� �Լ����� abstract �� �����߱� ������ ��ӹ��� ĳ���͵��� ����ִ� �Լ��� ����
        NormalAttackAction = NormalAttack;
        PassiveSkillAction = PassiveSkill;
        ActiveSkillAction = ActiveSkill;

        hp = characterData.data.healthPoint; //���� �ܿ� Hp�����Ͱ� ���⿡ HeroData�� �ִ� �ִ�ü�� ���

        animator.SetFloat("CharID", 0);
    }
    protected void FixedUpdate()
    {
        nowUpdate?.Invoke();
    }

    //Enemy�� Hero�� �������� ����ִ� �Լ�. Hero�� SetMoveNext�� SetRetrunPos �� �� ������ ����
    public abstract void SetBattle();

    //�ش��ϴ� ��ų���� ��ӹ��� ĳ���͵��� ������ �־�� ��
    //�ش��ϴ� ��ų�� �������� ���� NormalAttackAction �� �Ҵ��� �Լ��� �ٲ��ְ�����
    public abstract void NormalAttack();
    public abstract void PassiveSkill();
    public abstract void ActiveSkill();

    //���� �ִϸ��̼��� ������, ���¸� NormalAttack�� �ٲ��� �ӽ� �Լ�
    public abstract void NormalAttackEnd();
    public abstract void PassiveSkillEnd();
    public abstract void ActiveSkillEnd();

    //ĳ���͵��� ���¸����� ������Ʈ �Լ�. Enemy�� ������� �ʴ� ���¿�, Update�� ������ ����. �ƹ����X
    protected abstract void IdleUpdate();
    protected abstract void BattleUpdate();
    protected abstract void DieUpdate();
    protected abstract void MoveNextUpdate();
    protected abstract void ReturnPosUpdate();

    public abstract void OnDamage(int dmg, bool isCritical = false);
}