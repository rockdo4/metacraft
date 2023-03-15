using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAreaIndicator : MonoBehaviour
{
    public SkillTargetType TargetType { get; set; }
    public MeshRenderer Renderer { get; private set; }

    protected HashSet<Collider> unitsInArea = new();

    public bool isTriggerEnter = false;

    public bool IsTrackTarget { set { isTrackTarget = value; } }
    private bool isTrackTarget = false;
    public Transform TrackTransform { set { trackTransform = value; } }
    private Transform trackTransform;
 
    protected virtual void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        if (!isTrackTarget)
            return;

        if(trackTransform != null) //임시코드 영우형이 확인 예정
            transform.position = trackTransform.position + Vector3.up * 0.1f;
    }

    public virtual void SetScale(float x, float y, float z = 1f)
    {
        transform.localScale = new Vector3(x, y, z);        
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
        isTriggerEnter = true;
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
            if(unit != null)
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
