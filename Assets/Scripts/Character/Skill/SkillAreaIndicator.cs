using System.Collections.Generic;
using UnityEngine;

public class SkillAreaIndicator : MonoBehaviour
{
    public SkillTargetType TargetType { get; set; }

    private HashSet<Collider> unitsInArea = new();
    private void OnTriggerEnter(Collider other)
    {
        switch (TargetType)
        {
            case SkillTargetType.Enemy:
                if (other.CompareTag("Enemy"))
                {
                    IsColliderEnter(true, other);
                }
                break;
            case SkillTargetType.Friendly:
                if (other.CompareTag("Hero"))
                {
                    IsColliderEnter(true, other);
                }
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (TargetType)
        {
            case SkillTargetType.Enemy:
                if (other.CompareTag("Enemy"))
                {
                    IsColliderEnter(false, other);
                }
                break;
            case SkillTargetType.Friendly:
                if (other.CompareTag("Hero"))
                {
                    IsColliderEnter(false, other);
                }
                break;
        }
    }
    private void IsColliderEnter(bool enter, Collider other)
    {
        other.GetComponent<Outline>().enabled = enter; 

        if(enter)
            unitsInArea.Add(other);
        else
            unitsInArea.Remove(other);
    }
    private void OnDisable()
    {
        foreach (var unit in unitsInArea)
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
