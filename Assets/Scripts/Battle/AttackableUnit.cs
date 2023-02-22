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

    [SerializeField] //hp를 인스펙터에서 보려고 작성. 
    protected int hp;

    //쿨타임에 사용, Hero는 Ui버튼을 누를때 쿨타임을 검사하지만 Enemy나 Boss 가 생길수도 있으니 같이 작성해놓음
    protected float lastNormalAttackTime;
    protected float lastPassiveSkillTime;
    protected float lastActiveSkillTime;

    protected float lastNavTime;
    protected float navDelay = 0.2f;

    //현재 상태의 Update함수 호출
    protected Action nowUpdate;

    //각각의 캐릭터가 들고있는 스킬들, 나중에 각각의 스킬을들 제작하고, 해당 캐릭터가 밑의 이벤트에 할당해주는 방식으로 사용예정
    protected Action NormalAttackAction;
    protected Action PassiveSkillAction;
    protected Action ActiveSkillAction;

    protected virtual void Awake()
    {
        battleManager = GameObject.FindObjectOfType<BeltScrollBattleManager>();
    }

    // AttackableHero 와 AttackableEnemy 에서 프로퍼티 재정의
    // 상태가 겹치는게 많고, Enemy가 스킬을 가질 경우를 대비
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

        //이벤트 연결. 밑의 함수들은 abstract 로 선언했기 때문에 상속받은 캐릭터들이 들고있는 함수를 실행
        NormalAttackAction = NormalAttack;
        PassiveSkillAction = PassiveSkill;
        ActiveSkillAction = ActiveAttack;

        hp = heroData.stats.healthPoint; //현재 잔여 Hp데이터가 없기에 HeroData에 있는 최대체력 사용
    }
    protected void FixedUpdate()
    {
        nowUpdate?.Invoke();
    }

    //평타와 패시브 스킬 실시간으로 계산

    //Enemy와 Hero가 공통으로 들고있는 함수. Hero는 SetMoveNext와 SetRetrunPos 를 더 가지고 있음
    public abstract void SetBattle();

    //해당하는 스킬들을 상속받은 캐릭터들이 가지고 있어야 함
    //해당하는 스킬들 만들어놓고 위에 NormalAttackAction 에 할당할 함수를 바꿔주고싶음
    public abstract void NormalAttack();
    public abstract void PassiveSkill();
    public abstract void ActiveAttack();

    //현재 애니메이션이 없으니, 상태를 NormalAttack로 바꿔줄 임시 함수
    public abstract void TestPassiveEnd();
    public abstract void TestActiveEnd();

    //캐릭터들의 상태마다의 업데이트 함수. Enemy는 사용하지 않는 상태와, Update를 가질수 있음. 아무기능X
    protected abstract void IdleUpdate();
    protected abstract void BattleUpdate();
    protected abstract void DieUpdate();
    protected abstract void MoveNextUpdate();
    protected abstract void ReturnPosUpdate();

    public abstract void OnDamage(int dmg);
}
