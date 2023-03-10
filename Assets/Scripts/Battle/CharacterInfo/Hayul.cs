using System.Collections.Generic;

public class Hayul : AttackableHero
{
    public override void PassiveSkillEvent()
    {
        base.PassiveSkillEvent();
        Logger.Debug("하율등장");
        foreach (var hero in heroList)
        {
            foreach(var buff in passivekbuffs)
            {
                Logger.Debug("하율 버프사용");
                hero.AddBuff(buff, 0);
            }
        }
    }
    public override void NormalAttackOnDamage()
    {
        base.NormalAttackOnDamage();
    }
}
