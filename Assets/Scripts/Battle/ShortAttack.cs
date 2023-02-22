using System.Collections.Generic;
using UnityEngine;

public class ShortAttack : AttackableHero
{
    public override void NormalAttack()
    {
        base.NormalAttack();
        //Logger.Debug("Hero_NormalAttack");

        if (heroData.normalAttack.count == 1)
        {
            target.GetComponent<AttackableHero>().OnDamage(heroData.stats.baseDamage);
            return;
        }

        List<GameObject> attackHeros = new();

        foreach (var hero in targetList)
        {
            Vector3 interV = hero.transform.position - transform.position;
            if (interV.magnitude <= heroData.normalAttack.distance)
            {
                float dot = Vector3.Dot(interV.normalized, transform.forward);
                float theta = Mathf.Acos(dot);
                float degree = Mathf.Rad2Deg * theta;

                if (degree <= heroData.normalAttack.angle / 2f)
                    attackHeros.Add(hero.transform.gameObject);
            }
        }

        foreach (var hero in attackHeros)
        {
            hero.GetComponent<AttackableEnemy>().OnDamage(heroData.stats.baseDamage);
        }
    }
    public override void PassiveSkill()
    {
        base.PassiveSkill();
        //Logger.Debug("Hero_PassiveSkill");
    }

    protected override void SearchTarget()
    {
       SearchNearbyEnemy(); //근거리 타겟 추적
    }
    //private void OnDrawGizmos()
    //{
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, heroData.normalAttack.angle / 2, heroData.normalAttack.distance);
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -heroData.normalAttack.angle / 2, heroData.normalAttack.distance);
    //}
}
