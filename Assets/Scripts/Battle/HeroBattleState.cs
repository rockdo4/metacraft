public enum HeroState
{
    None = -1,
    Idle,
    ReturnPosition, // 재배치
    MoveNext,       // 다음지역으로 이동
    Battle,
    Die,
    Count
}

public enum HeroBattleState
{
    None,
    Normal,
    Ulitimate,
    Stun,
    Count
}