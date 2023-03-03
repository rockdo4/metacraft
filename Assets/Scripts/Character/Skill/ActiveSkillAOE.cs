using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillAOE", menuName = "Character/ActiveSkill/AOE")]
public class ActiveSkillAOE : CharacterSkill
{
    public SkillAreaIndicator skillAreaIndicator;    
    public LayerMask layerM;    
    public bool isInit = false;    

    private Camera cam;    

    public bool isAutoTargeting;
    public float castRangeLimit;
    public SkillAreaShape areaShapeType;
    public float areaRadiusOrRange;
    public float areaAngleOrWidth;
    public SkillTargetType targetType;
    public bool isCriticalPossible;
    public override void OnActive()
    {
        if (isInit)
            return;

        //layerM = 1 << 8;

        skillAreaIndicator = Instantiate(skillAreaIndicator);
        skillAreaIndicator.gameObject.SetActive(false);

        cam = Camera.main;

        isInit = true;
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
            skillAreaIndicator.transform.position = hit.point + Vector3.up * 0.1f;
        }        
    }
    public override void OnActiveSkill()
    {
        Logger.Debug(1111111111);

        var targets = skillAreaIndicator.GetUnitsInArea();

        foreach(var target in targets)
        {
            target.OnDamage(222);
        }

        skillAreaIndicator.gameObject.SetActive(false);
    }
    public override void SkillCancle()
    {
        skillAreaIndicator.gameObject.SetActive(false);
    }
}
