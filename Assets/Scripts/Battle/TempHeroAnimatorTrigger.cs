using UnityEngine;

public class TempHeroAnimatorTrigger : MonoBehaviour
{
    public AttackableUnit unit;
    public MakeWalkSound walkSound;

    private void Awake()
    {
        unit = transform.GetComponentInParent<AttackableUnit>();
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
    public void PlayFootStep()
    {
        walkSound.PlayFootStep();
    }
}