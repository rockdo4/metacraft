using UnityEngine;

[CreateAssetMenu(fileName = "AOEWithDuration", menuName = "Character/ActiveSkill/AOEDuration")]
public class AOEWithDuration : ActiveSkillAOE
{
    public float duration;
    public float hitInterval;

    public override void OnActiveSkill(LiveData data)
    {
        //EffectManager.Instance.Get(effectEnum, indicatorTransform);

        //var targets = skillAreaIndicator.GetUnitsInArea();

        //var damage = CreateDamageResult(data);

        //foreach (var target in targets)
        //{
        //    target.OnDamage(damage);
        //}

        //skillAreaIndicator.gameObject.SetActive(false);
        //skillAreaIndicator.isTriggerEnter = false;
    }
}
