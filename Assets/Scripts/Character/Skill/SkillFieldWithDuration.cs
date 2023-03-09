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

    private int damageResult;
    private float duration;
    private float hitInterval;
    private Transform createLocation;
    private SkillTargetType targetType;
    private SkillSearchType searchType;
    private EffectEnum effect;

    private float lastHitTime = 0f;

    private string offSkillFieldFuncName = nameof(OffSkillField);
    private void OnEnable()
    {
        Invoke(offSkillFieldFuncName, duration);
        EffectManager.Instance.Get(effect, transform);
    }
    private void OnTriggerStay(Collider other)
    {        
        if (Time.time - lastHitTime < hitInterval)
            return;

        lastHitTime = Time.time;
        transform.position = createLocation.position;

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
