public class Hayul : AttackableHero
{
    public override void PassiveSkillEvent()
    {
        base.PassiveSkillEvent(); //�нú� ���� ����� ����Ʈ ��������.
        Logger.Debug("��������");
        foreach (var hero in heroList)
        {
            foreach(var buff in passivekbuffs)
            {
                Logger.Debug("���� �������");
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
