public enum UnitType
{
    None = -1,
    Hero,
    Enemy,
    Count
}

public enum UnitAiType
{
    None = -1,
    Rush,
    Range,
    Assassin,
    Supprot,
    Count
}

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
    BattleIdle,
    NormalAttack,
    PassiveSkill,
    ActiveSkill,
    Stun,
    Count
}