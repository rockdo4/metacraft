using System.Collections;
using System.Net;
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

    public bool isAutoTargeting;
    public SkillAreaShape areaShapeType;
    public float areaRadiusOrRange;
    public float areaAngleOrWidth;    
    public bool isCriticalPossible;

    public float castRangeLimit = 10f;
    private float sqrCastRangeLimit;

    public override void OnActive()
    {
        if (skillAreaIndicator != null &&
            castRangeIndicator != null)
            return;

        //layerM = 1 << 8;

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
    private bool IsMouseInSkillRange(Vector3 hitPoint)
    {
        var x = actorTransform.position.x - hitPoint.x;
        var z = actorTransform.position.z - hitPoint.z;

        return x * x + z * z < sqrCastRangeLimit;
    }
    public override void OnActiveSkill(LiveData data)
    {      
        EffectManager.Instance.Get(effectEnum, indicatorTransform);

        var targets = skillAreaIndicator.GetUnitsInArea();

        var damage = CreateDamageResult(data);

        foreach (var target in targets)
        {            
            target.OnDamage(damage);
        }

        skillAreaIndicator.gameObject.SetActive(false);
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
