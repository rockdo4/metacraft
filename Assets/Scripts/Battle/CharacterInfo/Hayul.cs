public class Hayul : AttackableHero
{
    public override void PassiveSkillEvent()
    {
        base.PassiveSkillEvent(); //패시브 사용시 히어로 리스트 가져오기.
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

    public override void OnActiveSkill()
    {
        base.OnActiveSkill();
        var dmg = characterData.activeSkill.CreateDamageResult(characterData.data, bufferState);
        dmg = (int)(dmg * (100 + bufferState.damageDecrease) * 0.01f);
        foreach (var hero in heroList)
        {
            foreach (var buff in attackkbuffs)
            {
                hero.AddBuff(buff, dmg);
            }
        }
    }
}
