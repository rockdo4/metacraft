using System.Collections.Generic;
using UnityEngine;

public class SkillAreaIndicator : MonoBehaviour
{
    private HashSet<Collider> unitsInArea = new();    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            unitsInArea.Add(other);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            unitsInArea.Remove(other);
        }
    }
    public List<AttackableUnit> GetUnitsInArea()
    {
        List<AttackableUnit> container = new(unitsInArea.Count);

        foreach (Collider collider in unitsInArea)
        {
            AttackableUnit attackableUnit = collider.GetComponent<AttackableUnit>();
            container.Add(attackableUnit);
        }
        return container;
    }
}
