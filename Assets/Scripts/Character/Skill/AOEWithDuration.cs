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
        skillField.Duration = duration;
        skillField.HitInterval = hitInterval;
        skillField.Effect = activeEffect;
        skillField.IsInit = true;

        skillField.IsTrackTarget = isTrackTarget;        

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
    public override void OnActiveSkill(AttackableUnit attackableUnit)
    {
        skillEffectedUnits = skillAreaIndicator.GetUnitsInArea();

        skillField.transform.SetPositionAndRotation(indicatorTransform.position, indicatorTransform.rotation);
        skillField.TrackTransform = skillAreaIndicator.TrackTransform;
        skillField.gameObject.SetActive(true);

        bool isCiritical = false;
        skillField.DamageResult = attackableUnit.CalculDamage(this, ref isCiritical);
        skillField.SetAttackableData(ref attackableUnit, ref attackableUnit.GetUnitData().activeSkill);

        skillAreaIndicator.gameObject.SetActive(false);
        skillAreaIndicator.isTriggerEnter = false;
    }
}
