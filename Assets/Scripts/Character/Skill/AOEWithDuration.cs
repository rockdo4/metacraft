using UnityEngine;

[CreateAssetMenu(fileName = "AOEWithDuration", menuName = "Character/ActiveSkill/AOEDuration")]
public class AOEWithDuration : ActiveSkillAOE
{
    public float duration;
    public float hitInterval;

    public SkillFieldWithDuration skillFieldPrefab;
    private SkillFieldWithDuration skillField;

    public override void OnActive()
    {
        base.OnActive();

        if (skillField != null)
            return;

        skillField = Instantiate(skillFieldPrefab);
        skillField.TargetType = targetType;
        skillField.SearchType = searchType;
        skillField.gameObject.SetActive(false);
        skillField.CreateLocation = indicatorTransform;
        skillField.Duration= duration;
        skillField.HitInterval= hitInterval;
        skillField.Effect = effectEnum;
        SetFieldScale();
    }

    private void SetFieldScale()
    {
        switch (areaShapeType)
        {
            case SkillAreaShape.Sector:
                skillField.SetScale(sectorRadius, sectorAngle);
                break;
            case SkillAreaShape.Rectangle:
                skillField.SetScale(widthX, widthZ);
                break;
            case SkillAreaShape.Circle:
                skillField.SetScale(sectorRadius * 2, sectorRadius * 2);
                break;
        }
    }
    public override void OnActiveSkill(LiveData data)
    {
        skillField.transform.position = indicatorTransform.position;
        skillField.gameObject.SetActive(true);

        skillField.DamageResult = CreateDamageResult(data);

        skillAreaIndicator.gameObject.SetActive(false);
        skillAreaIndicator.isTriggerEnter = false;
    }
}
