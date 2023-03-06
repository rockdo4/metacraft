using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillAOE", menuName = "Character/ActiveSkill/AOE")]
public class ActiveSkillAOE : CharacterSkill
{
    public SkillAreaIndicator skillAreaIndicatorPrefab;
    private SkillAreaIndicator skillAreaIndicator;
    public LayerMask layerM;
    public ParticleSystem particle;

    private Camera cam;
    private Transform indicatorTransform;

    public bool isAutoTargeting;
    public float castRangeLimit;
    public SkillAreaShape areaShapeType;
    public float areaRadiusOrRange;
    public float areaAngleOrWidth;
    public SkillTargetType targetType;
    public bool isCriticalPossible;
    public override void OnActive()
    {
        if (skillAreaIndicator != null)
            return;

        //layerM = 1 << 8;

        skillAreaIndicator = Instantiate(skillAreaIndicatorPrefab);
        skillAreaIndicator.TargetType = targetType;
        skillAreaIndicator.gameObject.SetActive(false);
        indicatorTransform = skillAreaIndicator.transform;

        cam = Camera.main;
    }
    public override IEnumerator SkillCoroutine()
    {
        OnActive();

        skillAreaIndicator.gameObject.SetActive(true);

        while (true)
        {
            MoveIndicator();
            yield return null;
        }
    }
    private void MoveIndicator()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);        
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f, layerM))
        {
            indicatorTransform.position = hit.point + Vector3.up * 0.1f;
        }   
    } 
    public override void OnActiveSkill(int damage)
    {
        var particleContainer = Instantiate(particle);

        particleContainer.transform.position = indicatorTransform.position;

        var targets = skillAreaIndicator.GetUnitsInArea();

        if (targetType == SkillTargetType.Friendly)
            damage *= -1;

        foreach (var target in targets)
        {
            target.OnDamage(damage);
        }

        skillAreaIndicator.gameObject.SetActive(false);
    }
    public override void SkillCancle()
    {
        skillAreaIndicator?.gameObject.SetActive(false);
    }
}
