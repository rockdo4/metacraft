using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShortAttackEnemy : AttackableEnemy
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

        List<GameObject> attackHeroes = new();

        foreach (var hero in targetList)
        {
            Vector3 interV = hero.transform.position - transform.position;
            if (interV.magnitude <= heroData.normalAttack.distance)
            {
                float angle = Vector3.Angle(transform.forward, interV);

                if (Mathf.Abs(angle) < heroData.normalAttack.angle / 2f)
                {
                    attackHeroes.Add(hero.transform.gameObject);
                }
            }
        }

        foreach (var hero in attackHeroes)
        {
            hero.GetComponent<AttackableHero>().OnDamage(heroData.stats.baseDamage);
        }
    }
    public override void PassiveSkill()
    {
        base.PassiveSkill();
        //Logger.Debug("Enemy_PassiveSkill");
    }

    protected override void SearchTarget()
    {
        SearchNearbyEnemy(); //�ٰŸ� Ÿ�� ����
    }

    private void OnDrawGizmos()
    {
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, heroData.normalAttack.angle / 2, heroData.normalAttack.distance);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -heroData.normalAttack.angle / 2, heroData.normalAttack.distance);
    }
}
