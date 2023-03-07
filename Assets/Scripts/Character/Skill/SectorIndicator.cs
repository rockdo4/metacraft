using UnityEngine;

public class SectorIndicator : SkillAreaIndicator
{    
    private float angleHalf;
    protected override void Awake()
    {
        base.Awake();
        angleHalf = GetComponent<SectorMesh>().Angle * 0.5f;
    }
    protected override void IsColliderIn(bool isIn, Collider other)
    {
        Vector3 direction = other.transform.position - transform.position;
        var isInSector = isIn && Vector3.Angle(direction, transform.forward) <= angleHalf;

        other.GetComponent<Outline>().enabled = isInSector;

        if (isInSector)
            unitsInArea.Add(other);
        else
            unitsInArea.Remove(other);
    }
}
