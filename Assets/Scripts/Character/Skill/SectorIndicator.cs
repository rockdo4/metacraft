using System.Linq;
using UnityEngine;
public class SectorIndicator : SkillAreaIndicator
{
    private SectorMesh sectorMesh;
    private float angleHalf;
    private float radius;
    private float sqrRadius;
    private int layerMask;

    private Collider[] colliders;
    public int maxColliders = 32;
    protected override void Awake()
    {
        base.Awake();
        sectorMesh = GetComponent<SectorMesh>();
        colliders  = new Collider[maxColliders];
    }
    public override void SetScale(float x, float y, float z = 1)
    {
        sectorMesh.Radius = x;
        sectorMesh.Angle  = y;

        angleHalf = sectorMesh.Angle * 0.5f;
        radius    = sectorMesh.Radius;
        sqrRadius = radius * radius;
    }
    private void Start()
    {
        bool isTargetEnemy = TargetType == SkillTargetType.Enemy;
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        int heroLayerMask  = 1 << LayerMask.NameToLayer("Hero");
        layerMask          = isTargetEnemy ? enemyLayerMask : heroLayerMask;
    }
    private void FixedUpdate()
    {   
        maxColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, layerMask);
        for(int i = 0; i < maxColliders; i++)
        {
            IsColliderIn(IsInAngle(colliders[i]), colliders[i]);            
        }

        for (int i = unitsInArea.Count - 1; i >= 0; i--)
        {
            var elements = unitsInArea.ElementAt(i);
            if (!IsInAngle(elements) || !IsInRaidus(elements.transform.position))
            {
                IsColliderIn(false, elements);
            }
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
