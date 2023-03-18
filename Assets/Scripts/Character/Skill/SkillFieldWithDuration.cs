using System.Collections.Generic;
using UnityEngine;

public class SkillFieldWithDuration : MonoBehaviour
{   
    public int DamageResult { set { damageResult = value; } }
    public float Duration { set { duration = value; } }
    public float HitInterval { set { hitInterval = value; } }
    public Transform CreateLocation { set { createLocation = value; } }
    public SkillTargetType TargetType { set { targetType = value; } }
    public SkillSearchType SearchType { set { searchType = value; } }   
    public EffectEnum Effect { set { effect = value; } }
    public bool IsTrackTarget { set { isTrackTarget = value; } }
    public Transform TrackTransform { set { trackTransform = value; } }
    public bool IsInit = false;

    protected AttackableUnit attackableUnit;
    protected CharacterSkill skill;

    protected int damageResult;
    protected float duration;
    protected float hitInterval;
    protected Transform createLocation;
    protected SkillTargetType targetType;
    protected SkillSearchType searchType;
    protected EffectEnum effect;
    private bool isTrackTarget;
    private Transform trackTransform;

    protected float lastHitTime = 0f;
    
    protected string offSkillFieldFuncName = nameof(OffSkillField);

    private HashSet<Collider> colliders = new HashSet<Collider>();
    private string tagName;

    protected void OnEnable()
    {        
        Invoke(offSkillFieldFuncName, duration);
        if(IsInit)
        {
            var currEffect = EffectManager.Instance.Get(effect, transform);
            if (isTrackTarget)
                currEffect.transform.SetParent(transform, true);
        }
            
    }
    private void Start()
    {
        tagName = string.Empty;
        switch (targetType)
        {
            case SkillTargetType.Enemy:
                tagName = "Enemy";
                break;
            case SkillTargetType.Friendly:
                tagName = "Hero";
                break;
        }
    }
    public void SetAttackableData(ref AttackableUnit attackableUnit, ref CharacterSkill skill)
    {
        this.attackableUnit = attackableUnit;
        this.skill = skill;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            colliders.Remove(other);
        }
    }

    private void Update()
    {
        TrackTarget();
        OnDamage();
    }
    private void TrackTarget()
    {
        if(!isTrackTarget) 
            return;

        transform.position = trackTransform.position;
    }
    private void OnDamage()
    {
        if (Time.time - lastHitTime < hitInterval)
            return;

        lastHitTime = Time.time;

        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.gameObject.activeSelf)
            {
                var attackable = collider.GetComponent<AttackableUnit>();
                if(attackable.GetUnitState() == UnitState.Battle)
                    attackable.OnDamage(attackableUnit, skill);
            }
        }
    }
    public virtual void SetScale(float x, float y, float z = 1f)
    {
        transform.localScale = new Vector3(x, y, z);
    }
    private void OffSkillField()
    {
        gameObject.SetActive(false);
    }
}
