public enum HeroState
{
    None = -1,
    Idle,
    ReturnPosition, // ���ġ
    MoveNext,       // ������������ �̵�
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