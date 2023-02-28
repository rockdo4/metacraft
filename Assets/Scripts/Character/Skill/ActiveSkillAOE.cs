using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillAOE", menuName = "Character/ActiveSkill/AOE")]
public class ActiveSkillAOE : CharacterSkill
{
    public GameObject skillAreaIndicator;    
    private bool isInit = false;    
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
        if(!isInit)
        {
            skillAreaIndicator = Instantiate(skillAreaIndicator);
            skillAreaIndicator.SetActive(false);

            cam = Camera.main;

            isInit = true;
        }
    }
 
    public override IEnumerator SkillCoroutine()
    {
        skillAreaIndicator.SetActive(true);

        while (true)
        {
            MoveIndicator();
            yield return null;
        }
    }
    private void MoveIndicator()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100.0f, 8))
        {
            skillAreaIndicator.transform.position = new Vector3(hitInfo.point.x, 0.1f, hitInfo.point.z);
        }        
    }
}
