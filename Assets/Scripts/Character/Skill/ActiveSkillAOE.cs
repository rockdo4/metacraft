using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillAOE", menuName = "Character/ActiveSkill/AOE")]
public class ActiveSkillAOE : CharacterSkill
{
    public SkillAreaIndicator skillAreaIndicatorPrefab;
    private SkillAreaIndicator skillAreaIndicator;
    public LayerMask layerM;    

    public Transform ActorTransform { set { actorTransform = value; } }
    private Transform actorTransform;

    public GameObject castRangeIndicatorPrefab;
    private GameObject castRangeIndicator;
    private Transform castRangeIndicatorTransform;

    private Camera cam;
    private Transform indicatorTransform;

    public SkillAreaShape areaShapeType;

    public float sectorRadius;
    public float sectorAngle;

    public float widthZ;
    public float widthX;
    public bool isCriticalPossible;

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

        castRangeIndicator = Instantiate(castRangeIndicatorPrefab);
        castRangeIndicator.transform.localScale = castRangeLimit * 2 * Vector3.one;
        castRangeIndicator.SetActive(false);
        castRangeIndicatorTransform = castRangeIndicator.transform;

        cam = Camera.main;

        sqrCastRangeLimit = castRangeLimit * castRangeLimit;
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
    public override void OnActiveSkill(int damage, int level)
    {        
        EffectManager.Instance.Get(effectEnum, indicatorTransform);

        var targets = skillAreaIndicator.GetUnitsInArea();

        foreach (var target in targets)
        {            
            target.OnDamage(damage, level);
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
    public void OffIndicatorsForOnActiveSkill()
    {
        skillAreaIndicator.Renderer.enabled = false;
        castRangeIndicator.SetActive(false);
    }
}
