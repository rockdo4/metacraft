using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ShortAttack : AttackableHero
{
    public float angleRange = 30f;
    public float rad = 10f;

    public override void Attack()
    {
        base.Attack();
        Logger.Debug("Attack1111");
        Logger.Debug(charactorData.damage);

        if (charactorData.attackCount == 1)
        {
            Logger.Debug("Attack : " + target.name);
            return;
        }

        List<GameObject> attackEenmys = new();

        var Enemys = Physics.SphereCastAll(transform.position, rad, Vector3.up, 0f);
        foreach (var enemy in Enemys)
        {
            Vector3 interV = enemy.transform.position - transform.position;
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            float theta = Mathf.Acos(dot);
            float degree = Mathf.Rad2Deg * theta;

            if (degree <= angleRange / 2f)
                attackEenmys.Add(enemy.transform.gameObject);
        }

        foreach (var enemy in attackEenmys)
        {
            Logger.Debug("Attack : " + enemy.name);
        }
    }

    private void OnDrawGizmos()
    {
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, rad);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, rad);
    }

    public override void Skill()
    {
        base.Skill();
        Logger.Debug("Skill111");
    }
}
