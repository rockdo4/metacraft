using UnityEngine;

public class ForkedRoad : MonoBehaviour
{
    public ForkedRoadTrigger fadeTrigger;

    // ��ġ �� ���� ����
    public void SetRoadChangeAngle(Transform pos)
    {
        Utils.CopyPositionAndRotation(gameObject, pos);
    }
}
