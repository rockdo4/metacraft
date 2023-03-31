using UnityEngine;

public class TempHeroAnimatorTrigger : MonoBehaviour
{
    public AttackableUnit unit;
    private MakeWalkSound walkSound;

    private void Awake()
    {
        unit = transform.GetComponentInParent<AttackableUnit>();
        walkSound = transform.GetComponentInParent<MakeWalkSound>();
    }
    public void NormalAttackOnDamage(AnimationEvent ev)
    {
        unit.NormalAttackOnDamage();
    }
    public void DeadUnit(AnimationEvent ev)
    {
       // unit.OnDead(unit);
    }
    public void DestroyUnit(AnimationEvent ev)
    {
        unit.DestroyUnit();
    }
    public void OnActiveSkill(AnimationEvent ev)
    {
        unit.OnActiveSkill();
    }
    public void PlayNormalAttackSound()
    {
        unit.PlayNormalAttackSound();
    }
    public void PlayActiveSkillSound()
    {
        unit.PlayActiveSkillSound();
    }
    public void PlayFootStep()
    {
        if(walkSound != null)
            walkSound.PlayFootStep();
    }
}