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
    Count
}

public enum UnitState
{
    None = -1,
    Idle,
    ReturnPosition, // ���ġ
    MoveNext,       // ������������ �̵�
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