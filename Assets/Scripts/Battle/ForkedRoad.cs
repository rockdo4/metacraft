using UnityEngine;

public class ForkedRoad : MonoBehaviour
{
    public MapEventTrigger fadeTrigger;

    // 위치 및 각도 조정
    public void SetRoadChangeAngle(Transform pos)
    {
        Utils.CopyPositionAndRotation(gameObject, pos);
    }
}
