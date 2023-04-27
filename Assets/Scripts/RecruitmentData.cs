using UnityEngine;

[CreateAssetMenu(fileName = "RecruitmentData", menuName = "Recruitment/RecruitmentData")]
public class RecruitmentData : ScriptableObject
{
    public string type;            // ��í����
    public string image;           // ��í ������ �´� ����
    public float aGradeRate;       // A��� Ȯ��
    public float bGradeRate;       // B��� Ȯ��
    public float cGradeRate;       // C��� Ȯ��
}
