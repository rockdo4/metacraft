using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillAOE", menuName = "Character/ActiveSkill/AOE")]
public class ActiveSkillAOE : CharacterSkill
{
    public SkillAreaIndicator skillAreaIndicator;    
    private bool isInit = false;    
    private Camera cam;
    private int layerMask;

    public bool isAutoTargeting;
    public float castRangeLimit;
    public SkillAreaShape areaShapeType;
    public float areaRadiusOrRange;
    public float areaAngleOrWidth;
    public SkillTargetType targetType;
    public bool isCriticalPossible;
    public override void OnActive()
    {
        if(!isInit)
        {
            layerMask = 1 << 8;

            skillAreaIndicator = Instantiate(skillAreaIndicator);            
            skillAreaIndicator.gameObject.SetActive(false);

            cam = Camera.main;
 
            isInit = true;
        }
    } 
    public override IEnumerator SkillCoroutine()
    {
        skillAreaIndicator.gameObject.SetActive(true);

        while (true)
        {
            if (TryActiveSkill())
                yield break;

            MoveIndicator();
            yield return null;
        }
    }
    private void MoveIndicator()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
        {            
            skillAreaIndicator.transform.position = hit.point + Vector3.up * 0.1f;
        }        
    }
    private bool TryActiveSkill()
    {
        if(Input.GetMouseButtonUp(0))
        {
            ActiveSkill();          
            return true;
        }
        return false;
    }
    private void ActiveSkill()
    {
        var targets = skillAreaIndicator.GetUnitsInArea();

        foreach(var target in targets)
        {
            target.OnDamage(222);
        }

        skillAreaIndicator.gameObject.SetActive(false);
    }
}
