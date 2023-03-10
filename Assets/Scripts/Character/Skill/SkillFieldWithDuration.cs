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
    
    protected float lastHitTime = 0f;
    
    protected string offSkillFieldFuncName = nameof(OffSkillField);
    protected void OnEnable()
    {        
        Invoke(offSkillFieldFuncName, duration);
        if(IsInit)
            EffectManager.Instance.Get(effect, transform);
    }

    public void SetAttackableData(ref AttackableUnit attackableUnit, ref CharacterSkill skill)
    {
        this.attackableUnit = attackableUnit;
        this.skill = skill;
    }

    private void OnTriggerStay(Collider other)
    {   
        if (Time.time - lastHitTime < hitInterval)
            return;

        lastHitTime = Time.time;        

        switch (targetType)
        {
            case SkillTargetType.Enemy:
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<AttackableUnit>().OnDamage(damageResult);
                }
                break;
            case SkillTargetType.Friendly:
                if (other.CompareTag("Hero"))
                {
                    other.GetComponent<AttackableUnit>().OnDamage(damageResult);
                }
                break;
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
