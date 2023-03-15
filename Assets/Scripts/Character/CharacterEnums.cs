public enum CharacterGrade
{
    None = 0,
    C,
    B,
    A,
    S,
    SS,
}

public enum UnitType
{
    None = -1,
    Hero,
    Enemy,
    Count
}

public enum CharacterJob
{
    None = 0,
    Assult,
    Defence,
    Shooter,
    Assassin,
    Support,
    Villain,
    Count
}
// ����, ���, ���, ����, ����, ����

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