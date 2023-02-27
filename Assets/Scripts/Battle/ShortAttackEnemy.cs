using System.Collections.Generic;
using System.Linq;
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

        foreach (var hero in heroList)
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

        attackHeroes.OrderBy(t => Vector3.Distance(transform.position, t.transform.position));

        var cnt = Mathf.Min(attackHeroes.Count, characterData.attack.count);
        for (int i = 0; i < cnt; i++)
        {
            attackHeroes[i].GetComponent<AttackableHero>().OnDamage(characterData.data.baseDamage);
        }
    }
    public override void PassiveSkill()
    {
        base.PassiveSkill();
        //Logger.Debug("Enemy_PassiveSkill");
    }

    protected override void SearchTarget()
    {
        SearchNearbyTarget(heroList); //근거리 타겟 추적
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, characterData.attack.angle / 2, characterData.attack.distance);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -characterData.attack.angle / 2, characterData.attack.distance);
    }
#endif
}
