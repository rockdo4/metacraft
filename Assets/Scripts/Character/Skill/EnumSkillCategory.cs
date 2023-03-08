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
    Targeting,  //���ϱ�
    Buffer,
    Healer,
    Another,
    Count,
}
public enum SkillTargetType
{
    None = -1,
    Enemy,
    Friendly,
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