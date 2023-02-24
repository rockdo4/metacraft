using UnityEngine;

public class TempHeroAnimatorTrigger : MonoBehaviour
{
    public ShortAttackHero hero;

    public void HeroPassiveSkill(AnimationEvent ev)
    {
        hero.HeroPassiveSkill();
    }
    public void HeroActiveSkill(AnimationEvent ev)
    {
        hero.HeroActiveSkill();
    }
    public void TestSkillEnd(AnimationEvent ev)
    {
        hero.TestSkillEnd();
    }
    public void DeadHero(AnimationEvent ev)
    {
        hero.DeadHero();
    }
    public void DestroyHero(AnimationEvent ev)
    {
        hero.DestroyHero();
    }
}
