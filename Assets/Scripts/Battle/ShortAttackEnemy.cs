using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShortAttackEnemy : AttackableEnemy
{
    public override void NormalAttack()
    {
        base.NormalAttack();
        //Logger.Debug("Hero_NormalAttack");

        if (characterData.attack.count == 1)
        {
            target.GetComponent<AttackableHero>().OnDamage(characterData.data.baseDamage);
            return;
        }

        List<GameObject> attackHeroes = new();

        foreach (var hero in targetList)
        {
            Vector3 interV = hero.transform.position - transform.position;
            if (interV.magnitude <= characterData.attack.distance)
            {
                float angle = Vector3.Angle(transform.forward, interV);

                if (Mathf.Abs(angle) < characterData.attack.angle / 2f)
                {
                    attackHeroes.Add(hero.transform.gameObject);
                }
            }
        }

        foreach (var hero in attackHeroes)
        {
            hero.GetComponent<AttackableHero>().OnDamage(characterData.data.baseDamage);
        }
    }
    public override void PassiveSkill()
    {
        base.PassiveSkill();
        //Logger.Debug("Enemy_PassiveSkill");
    }

    protected override void SearchTarget()
    {
        SearchNearbyEnemy(); //근거리 타겟 추적
    }

    //private void OnDrawGizmos()
    //{
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, characterData.attack.angle / 2, characterData.attack.distance);
    //    Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -characterData.attack.angle / 2, characterData.attack.distance);
    //}
}
