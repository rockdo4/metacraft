using UnityEngine;

public class ForkedRoad : MonoBehaviour
{
    public MapEventTrigger fadeTrigger;

    // ��ġ �� ���� ����
    public void SetRoadChangeAngle(Transform pos)
    {
        Utils.CopyPositionAndRotation(gameObject, pos);
    }
}
