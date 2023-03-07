using UnityEngine;
public class SectorIndicator : SkillAreaIndicator
{    
    private float angleHalf;
    private float radius;
    private float sqrRadius;
    protected override void Awake()
    {
        base.Awake();
        angleHalf = GetComponent<SectorMesh>().Angle * 0.5f;
        radius = GetComponent<SectorMesh>().Radius;
        sqrRadius = radius * radius;
    }
    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphereNonAlloc(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (!collider.CompareTag("Enemy"))
                continue;

            IsColliderIn(IsInAngle(collider), collider);
        }

        foreach(var collider in unitsInArea)
        {
            var isIn = IsInAngle(collider) && IsInRaidus(collider.transform.position);
            IsColliderIn(isIn, collider);
        }
    }
    private bool IsInAngle(Collider collider)
    {
        Vector3 direction = (collider.transform.position - transform.position).normalized;
        return  Vector3.Angle(direction, transform.forward) <= angleHalf;
    }
    private bool IsInRaidus(Vector3 pos)
    {
        var x = transform.position.x - pos.x;
        var z = transform.position.z - pos.z;

        return x * x + z * z < sqrRadius;
    }
}
