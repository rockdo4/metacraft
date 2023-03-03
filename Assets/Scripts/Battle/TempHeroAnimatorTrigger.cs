using UnityEngine;

public class TempHeroAnimatorTrigger : MonoBehaviour
{
    public AttackableUnit unit;

    public void NormalAttack(AnimationEvent ev)
    {
        unit.NormalAttack();
    }
    public void PassiveSkill(AnimationEvent ev)
    {
        unit.PassiveSkill();
    }
    public void ActiveSkill(AnimationEvent ev)
    {
        unit.ReadyActiveSkill();
    }
    public void NormalAttackEnd(AnimationEvent ev)
    {
        unit.NormalAttackEnd();
    }
    public void PassiveSkillEnd(AnimationEvent ev)
    {
        unit.PassiveSkillEnd();
    }
    public void ActiveSkillEnd(AnimationEvent ev)
    {
        unit.ActiveSkillEnd();
    }
    public void DeadUnit(AnimationEvent ev)
    {
        unit.OnDead(unit);
    }
    public void DestroyUnit(AnimationEvent ev)
    {
        unit.DestroyUnit();
    }
    public void OnActiveSkill(AnimationEvent ev)
    {
        unit.OnActiveSkill();
    }
}
