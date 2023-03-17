public enum SkillMainType
{
    None = -1,
    BaseAttack = 1,
    Passive,
    Active,
    CountPlusOne,
}
public enum SkillAreaShape
{
    None = -1,
    Sector,
    Rectangle,
    Circle,
    Count,
}
public enum SkillSearchType
{
    None = -1,
    AOE,
    Targeting,  //¥‹¿œ±‚
    Buffer,
    Healer,
    Another,
    Count,
}
public enum SkillTargetType
{
    None = -1,
    Friendly,
    Enemy,
    Both,
    Count,
}
public enum BufferTargetType
{
    None = -1,
    Self,
    Friendly,
    Enemy,
    Both,
    Count,
}
public enum SkillCoefficientType
{
    None = -1,
    Attack,
    Defense,
    MaxHealth,
    Health,
    Count,
}
public enum SkillIndicatorTarget
{
    None = -1,
    Self,
    Enemy,
    Friendly,
    Count,
}