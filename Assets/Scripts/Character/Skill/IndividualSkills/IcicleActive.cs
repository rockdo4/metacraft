
using UnityEngine;

public class IcicleActive : SkillFieldWithDuration
{
    public float startDelay = 0.6f;
    private float startDelayCheckTimer = 0f;
    protected override void OnEnable()
    {
        startDelayCheckTimer = 0f;        
        
        if (IsInit)
        {
            Invoke(offSkillFieldFuncName, duration + startDelay);
            EffectManager.Instance.Get(effect, transform);
        }
    }

    protected override void Update()
    {
        startDelayCheckTimer += Time.deltaTime;

        if (startDelayCheckTimer < startDelay)
            return;

        OnDamage();
    }

    protected override void OffSkillField()
    {        
        gameObject.SetActive(false);
    }
}
