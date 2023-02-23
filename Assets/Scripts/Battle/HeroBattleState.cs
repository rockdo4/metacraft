public enum UnitState
{
    None = -1,
    Idle,
    ReturnPosition, // 재배치
    MoveNext,       // 다음지역으로 이동
    Battle,
    Die,
    Count
}

public enum UnitBattleState
{
    None = -1,
    MoveToTarget,
    NormalAttack,
    PassiveSkill,
    ActiveSkill,
    Stun,
    Count
}