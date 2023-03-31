
using UnityEngine;

public class IcicleActive : SkillFieldWithDuration
{
    public float startDelay = 0.5f;
    public float startDelayCheckTimer = 0f;
    protected override void OnEnable()
    {
        Invoke(offSkillFieldFuncName, duration + startDelay);
    }

    protected override void Update()
    {
        startDelayCheckTimer += Time.deltaTime;

        OnDamage();
    }

    protected override void OffSkillField()
    {
        gameObject.SetActive(false);
    }
}
