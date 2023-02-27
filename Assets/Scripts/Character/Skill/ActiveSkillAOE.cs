using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillAOE", menuName = "Character/ActiveSkill/AOE")]
public class ActiveSkillAOE : CharacterSkill
{
    private Coroutine coShowArea;

    public bool isAutoTargeting;
    public float castRangeLimit;
    public SkillAreaShape areaShapeType;
    public float areaRadiusOrRange;
    public float areaAngleOrWidth;
    public SkillTargetType targetType;
    public bool isCriticalPossible;
    public override void OnActive()
    {
        
    }
    public override IEnumerator SkillCoroutine()
    {
        while(true)
        {            
            yield return null;
        }
    }
}
