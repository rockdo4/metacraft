using Unity.Burst.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillAOE", menuName = "Character/ActiveSkill/AOEStartFromCharacter")]
public class AOEStartFromCharacter : ActiveSkillAOE
{
    protected override void MoveSkillAreaIndicator()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, layerM))
        {
            indicatorTransform.position = actorTransform.position + Vector3.up * 0.1f;
            indicatorTransform.LookAt(hit.point);
        }
    }
    protected override void MoveSkillToTargetAreaIndicator()
    {
        indicatorTransform.position = actorTransform.position + Vector3.up * 0.1f;
        indicatorTransform.LookAt(targetPos);
    }
}
