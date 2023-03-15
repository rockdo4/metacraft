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
                hero.AddValueBuff(buff);
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
        dmg = (int)(dmg * bufferState.damageDecrease);
        foreach (var hero in heroList)
        {
            foreach (var buff in attackkbuffs)
            {
                hero.AddValueBuff(buff, dmg);
            }
        }
    }
}
