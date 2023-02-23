using System.Collections.Generic;
using UnityEngine;

public class RangeAttackHero : AttackableHero
{
    protected override void SearchTarget()
    {

    }

    public override void NormalAttack()
    {
        base.NormalAttack();
        //Logger.Debug("Hero_NormalAttack");

            target.GetComponent<AttackableHero>().OnDamage(characterData.data.baseDamage);
            return;
    }
    public override void PassiveSkill()
    {
        base.PassiveSkill();
    }
}
