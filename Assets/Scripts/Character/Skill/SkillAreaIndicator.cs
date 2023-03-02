using System.Collections.Generic;
using UnityEngine;

public class SkillAreaIndicator : MonoBehaviour
{
    private HashSet<Collider> unitsInArea = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Outline>().enabled = true;
            unitsInArea.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Outline>().enabled = false;
            unitsInArea.Remove(other);
        }
    }
    private void OnDisable()
    {
        foreach(var unit in unitsInArea)
        {
            unit.GetComponent<Outline>().enabled = false;
        }
        unitsInArea.Clear();
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
