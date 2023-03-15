using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillAOE", menuName = "Character/ActiveSkill/AOE")]
public class ActiveSkillAOE : CharacterSkill
{
    public SkillAreaIndicator skillAreaIndicatorPrefab;
    protected SkillAreaIndicator skillAreaIndicator;
    public LayerMask layerM;
    public Transform ActorTransform { set { actorTransform = value; } }
    protected Transform actorTransform;

    public GameObject castRangeIndicatorPrefab;
    private GameObject castRangeIndicator;
    private Transform castRangeIndicatorTransform;

    protected Camera cam;
    protected Transform indicatorTransform;

    public SkillAreaShape areaShapeType;
    public SkillIndicatorTarget indicatorTargetWhenTrackTarget;
    public bool isTrackTarget;
    private LayerMask targetMaskWhenManualSkill;

    public float sectorRadius;
    public float sectorAngle;

    public float widthZ;
    public float widthX;    

    public float castRangeLimit = 10f;
    private float sqrCastRangeLimit;

    private bool skillStartFromCharacter;

    private Collider[] colliders;
    public int maxColliders = 32;
    private Transform closestEnemyTransformWhenTrackTarget;
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

        if (areaShapeType.Equals(SkillAreaShape.Sector))
            skillStartFromCharacter = true;

        if (isTrackTarget)
        {
            InitTrackTargetLayerMaskSet();
            colliders = new Collider[maxColliders];
            skillAreaIndicator.IsTrackTarget = true;
        }
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
            MoveSkillAreaIndicator();
            yield return null;
        }
    }
    private void MoveCastRangeIndicator()
    {
        castRangeIndicatorTransform.position = actorTransform.position + Vector3.up * 0.1f; ;
    }
    protected virtual void MoveSkillAreaIndicator()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, layerM))
        {
            if (!isTrackTarget)
                WhenManualTarget(hit);
            else
                WhenTrackTarget(hit);
        }
    }
    private void WhenManualTarget(RaycastHit hit)
    {
        if (!skillStartFromCharacter)
            IndicatorPosToMouse(hit);
        else
            IndicatorOnlyChangeRotation(hit);
    }
    private void InitTrackTargetLayerMaskSet()
    {
        switch(indicatorTargetWhenTrackTarget)
        {
            case SkillIndicatorTarget.Self:
                break;
            case SkillIndicatorTarget.Enemy:
                targetMaskWhenManualSkill = 1 << LayerMask.NameToLayer("Enemy");                
                break;
            case SkillIndicatorTarget.Friendly:
                targetMaskWhenManualSkill = 1 << LayerMask.NameToLayer("Hero");
                break;
        }        
    }
    private void WhenTrackTarget(RaycastHit hit)
    {        
        //switch(indicatorTargetWhenAutoTarget)
        //{
        //    case SkillIndicatorTarget.Self:
        //        break;
        //    case SkillIndicatorTarget.Enemy:                
        //        break;
        //    case SkillIndicatorTarget.Friendly:
        //        break;
        //}
        IndicatorPosTrack(hit);
    }
    private void IndicatorPosTrack(RaycastHit hit)
    {
        var target = isAuto ? targetPos : hit.point;

        maxColliders = Physics.OverlapSphereNonAlloc(actorTransform.position, castRangeLimit, colliders, targetMaskWhenManualSkill);
        
        if (maxColliders.Equals(0))
            return;

        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < maxColliders; i++)
        {
            float distanceSQR = GetDistanceNoneSQRT(target, colliders[i].transform.position);
            if (distanceSQR < closestDistance)
            {
                closestDistance = distanceSQR;
                closestEnemyTransformWhenTrackTarget = colliders[i].transform;
            }
        }
        
        skillAreaIndicator.TrackTransform = closestEnemyTransformWhenTrackTarget;
    }

    private void IndicatorPosToMouse(RaycastHit hit)
    {
        var target = isAuto ? targetPos : hit.point;

        if (IsTargetInRange(target))
        {
            indicatorTransform.position = target + Vector3.up * 0.1f;
        }
        else
        {
            Vector3 point
                = Utils.IntersectPointCircleCenterToOut(actorTransform.position, castRangeLimit, target);

            indicatorTransform.position = point + Vector3.up * 0.1f;
        }
    }
    private void IndicatorOnlyChangeRotation(RaycastHit hit)
    {
        var target = isAuto ? targetPos : hit.point;

        indicatorTransform.position = actorTransform.position + Vector3.up * 0.1f;        
        
        float currentRotationX = indicatorTransform.eulerAngles.x;        
        indicatorTransform.LookAt(target + Vector3.up * 0.1f);        
        Vector3 newRotation = indicatorTransform.eulerAngles;
        newRotation.x = currentRotationX;
        indicatorTransform.eulerAngles = newRotation;
    }

    protected bool IsTargetInRange(Vector3 hitPoint)
    {
        var x = actorTransform.position.x - hitPoint.x;
        var z = actorTransform.position.z - hitPoint.z;

        return x * x + z * z < sqrCastRangeLimit;
    }
    protected float GetDistanceNoneSQRT(Vector3 pos1, Vector3 pos2)
    {
        var x = pos1.x - pos2.x;
        var z = pos1.z - pos2.z;

        return x * x + z * z;
    }

    public override void OnActiveSkill(AttackableUnit attackableUnit)
    {
        if(skillStartFromCharacter)
            EffectManager.Instance.Get(activeEffect, indicatorTransform, indicatorTransform.rotation);
        else
            EffectManager.Instance.Get(activeEffect, indicatorTransform);

        skillEffectedUnits = skillAreaIndicator.GetUnitsInArea();

        foreach (var target in skillEffectedUnits)
        {            
            target.OnDamage(attackableUnit, this);
            foreach (var buff in buffInfos)
            {
                target.AddBuff(buff, 0);
            }
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
    public void ReadyEffectUntillOnActiveSkill()
    {
        EffectManager.Instance.Get(readyEffect, indicatorTransform);
        skillAreaIndicator.Renderer.enabled = false;
        castRangeIndicator.SetActive(false);
    }
}
