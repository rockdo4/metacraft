public class ShortAttack : AttackableHero
{
    public override void Attack()
    {
        base.Attack();
        Logger.Debug("Attack1111");
    }
    public override void Skill()
    {
        base.Skill();
        Logger.Debug("Skill111");
    }
}
