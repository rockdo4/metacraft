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
// 돌격, 방어, 사격, 은밀, 지원, 빌런

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