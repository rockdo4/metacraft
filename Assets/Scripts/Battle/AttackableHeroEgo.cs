public class AttackableHeroEgo : AttackableHero
{
    public override void NormalAttackOnDamage()
    {
        bufferState.energyCnt++;
        base.NormalAttackOnDamage();
    }

    public override void OnActiveSkill()
    {
        characterData.activeSkill.OnActiveSkill(this, enemyList, heroList);
    }
}
