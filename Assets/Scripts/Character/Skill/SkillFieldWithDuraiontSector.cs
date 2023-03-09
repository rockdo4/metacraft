using UnityEngine;

public class SkillFieldWithDuraiontSector : SkillFieldWithDuration
{
    private LayerMask layerMask;
    private float angleHalf;
    private float radius;
    private float sqrRadius;

    private Collider[] colliders;
    public int maxColliders = 32;
    private void Start()
    {
        bool isTargetEnemy = targetType == SkillTargetType.Enemy;
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        int heroLayerMask = 1 << LayerMask.NameToLayer("Hero");
        layerMask = isTargetEnemy ? enemyLayerMask : heroLayerMask;
    }
    private void Update()
    {
        if (Time.time - lastHitTime < hitInterval)
            return;

        lastHitTime = Time.time;

        maxColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, layerMask);
        for (int i = 0; i < maxColliders; i++)
        {
            if (IsInAngle(colliders[i]) && IsInRaidus(colliders[i].transform.position))
            {
                colliders[i].GetComponent<AttackableUnit>().OnDamage(damageResult);
            }
        }
    }
    public override void SetScale(float x, float y, float z = 1)
    {
        angleHalf = y;
        radius = x;
        sqrRadius = radius * radius;
    }
    private bool IsInAngle(Collider collider)
    {
        Vector3 direction = (collider.transform.position - transform.position).normalized;
        return Vector3.Angle(direction, transform.forward) <= angleHalf;
    }
    private bool IsInRaidus(Vector3 pos)
    {
        var x = transform.position.x - pos.x;
        var z = transform.position.z - pos.z;

        return x * x + z * z < sqrRadius;
    }
}
