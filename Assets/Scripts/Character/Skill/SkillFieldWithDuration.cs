using System.Collections.Generic;
using System.Linq;
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
    protected GameObject currParticle;

    public AudioSource[] activeSkillAttackHitSounds;

    protected virtual void OnEnable()
    {        
        Invoke(offSkillFieldFuncName, duration);
        if(IsInit)
        {
            currParticle = EffectManager.Instance.Get(effect, transform).transform.GetChild(0).gameObject;            
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

    protected virtual void Update()
    {
        if(isTrackTarget)
            currParticle.transform.position = transform.position;
        TryTrackTarget();
        OnDamage();
    }
    protected void TryTrackTarget()
    {
        if(!isTrackTarget) 
            return;

        transform.position = trackTransform.position;
    }
    protected void OnDamage()
    {
        if (Time.time - lastHitTime < hitInterval)
            return;

        lastHitTime = Time.time;    
  
        for(int i = colliders.Count - 1; i >= 0; --i)
        {
            var value = colliders.ElementAt(i);

            if(value.Equals(null) || !value.gameObject.activeSelf)
            {
                colliders.Remove(value);
            }
            else
            {
                var attackable = value.GetComponent<AttackableUnit>();
                if (attackable.GetUnitState().Equals(UnitState.Battle))
                    attackable.OnDamage(attackableUnit, skill);
            }
        }
        
        if (colliders.Count > 0 && activeSkillAttackHitSounds.Length > 0)
            activeSkillAttackHitSounds[Random.Range(0, activeSkillAttackHitSounds.Length)].Play();
    }
    public virtual void SetScale(float x, float y, float z = 1f)
    {
        transform.localScale = new Vector3(x, y, z);
    }
    protected virtual void OffSkillField()
    {
        gameObject.SetActive(false);
    }
}
