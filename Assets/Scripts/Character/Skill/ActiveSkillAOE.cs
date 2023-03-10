using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillAOE", menuName = "Character/ActiveSkill/AOE")]
public class ActiveSkillAOE : CharacterSkill
{
    public SkillAreaIndicator skillAreaIndicatorPrefab;
    protected SkillAreaIndicator skillAreaIndicator;
    public LayerMask layerM;    

    public Transform ActorTransform { set { actorTransform = value; } }
    private Transform actorTransform;

    public GameObject castRangeIndicatorPrefab;
    private GameObject castRangeIndicator;
    private Transform castRangeIndicatorTransform;

    private Camera cam;
    protected Transform indicatorTransform;

    public SkillAreaShape areaShapeType;

    public float sectorRadius;
    public float sectorAngle;

    public float widthZ;
    public float widthX;    

    public float castRangeLimit = 10f;
    private float sqrCastRangeLimit;
    public override void OnActive()
    {
        if (skillAreaIndicator != null &&
            castRangeIndicator != null)
            return;

        skillAreaIndicator = Instantiate(skillAreaIndicatorPrefab);
        skillAreaIndicator.TargetType = targetType;
        skillAreaIndicator.gameObject.SetActive(false);
        indicatorTransform = skillAreaIndicator.transform;

        SetIndicatorScale();

        castRangeIndicator = Instantiate(castRangeIndicatorPrefab);
        castRangeIndicator.transform.localScale = castRangeLimit * 2 * Vector3.one;
        castRangeIndicator.SetActive(false);
        castRangeIndicatorTransform = castRangeIndicator.transform;

        cam = Camera.main;

        sqrCastRangeLimit = castRangeLimit * castRangeLimit;
    }
    protected virtual void SetIndicatorScale()
    {
        switch(areaShapeType)
        {
            case SkillAreaShape.Sector:
                skillAreaIndicator.SetScale(sectorRadius, sectorAngle);
                break;
            case SkillAreaShape.Rectangle:
                skillAreaIndicator.SetScale(widthX, widthZ);
                break;
            case SkillAreaShape.Circle:
                skillAreaIndicator.SetScale(sectorRadius * 2, sectorRadius * 2);
                break;
        }
    }
    public override IEnumerator SkillCoroutine()
    {
        OnActive();

        SetActiveIndicators(true);

        while (true)
        {
            MoveCastRangeIndicator();

            if (isAuto)
                MoveSkillToTargetAreaIndicator();
            else
                MoveSkillAreaIndicator();

            yield return null;
        }
    }
    private void MoveCastRangeIndicator()
    {
        castRangeIndicatorTransform.position = actorTransform.position + Vector3.up * 0.1f; ;
    }
    private void MoveSkillAreaIndicator()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, layerM))
        {            
            if (IsMouseInSkillRange(hit.point))
            {
                indicatorTransform.position = hit.point + Vector3.up * 0.1f;                
            }
            else
            {
                Vector3 point
                    = Utils.IntersectPointCircleCenterToOut(actorTransform.position, castRangeLimit, hit.point);
                
                indicatorTransform.position = point + Vector3.up * 0.1f;
            }
        }
    }
    private void MoveSkillToTargetAreaIndicator()
    {
        if (IsMouseInSkillRange(targetPos))
        {
            indicatorTransform.position = targetPos + Vector3.up * 0.1f;
        }
        else
        {
            Vector3 point
                = Utils.IntersectPointCircleCenterToOut(actorTransform.position, castRangeLimit, targetPos);

            indicatorTransform.position = point + Vector3.up * 0.1f;
        }
    }
    private bool IsMouseInSkillRange(Vector3 hitPoint)
    {
        var x = actorTransform.position.x - hitPoint.x;
        var z = actorTransform.position.z - hitPoint.z;

        return x * x + z * z < sqrCastRangeLimit;
    }
    public override void OnActiveSkill(AttackableUnit attackableUnit)
    {        
        EffectManager.Instance.Get(activeEffect, indicatorTransform);

        var targets = skillAreaIndicator.GetUnitsInArea();

        foreach (var target in targets)
        {            
            target.OnDamage(attackableUnit, this);
        }

        skillAreaIndicator.gameObject.SetActive(false);
        skillAreaIndicator.isTriggerEnter = false;
    }
    public void SetActiveIndicators(bool active)
    {
        skillAreaIndicator?.gameObject.SetActive(active);
        castRangeIndicator?.SetActive(active);
    }
    public void ActiveOffSkillAreaIndicator()
    {
        skillAreaIndicator?.gameObject.SetActive(false);
    }
    public void readyEffectUntillOnActiveSkill()
    {
        EffectManager.Instance.Get(readyEffect, indicatorTransform);
        skillAreaIndicator.Renderer.enabled = false;
        castRangeIndicator.SetActive(false);
    }
}
