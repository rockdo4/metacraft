using System.Collections.Generic;
using UnityEngine;

public class SkillAreaIndicator : MonoBehaviour
{
    public SkillTargetType TargetType { get; set; }
    public MeshRenderer Renderer { get; private set; }

    protected HashSet<Collider> unitsInArea = new();
    protected virtual void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        //if(meshRenderer == null)
        //    meshRenderer = GetComponent<MeshRenderer>();

        Renderer.enabled = true;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        switch (TargetType)
        {
            case SkillTargetType.Enemy:
                if (other.CompareTag("Enemy"))
                {
                    IsColliderIn(true, other);
                }
                break;
            case SkillTargetType.Friendly:
                if (other.CompareTag("Hero"))
                {
                    IsColliderIn(true, other);
                }
                break;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        switch (TargetType)
        {
            case SkillTargetType.Enemy:
                if (other.CompareTag("Enemy"))
                {
                    IsColliderIn(false, other);
                }
                break;
            case SkillTargetType.Friendly:
                if (other.CompareTag("Hero"))
                {
                    IsColliderIn(false, other);
                }
                break;
        }
    }
    protected virtual void IsColliderIn(bool isIn, Collider other)
    {
        other.GetComponent<Outline>().enabled = isIn;

        if (isIn)
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