using UnityEngine;

[CreateAssetMenu(fileName = "RecruitmentData", menuName = "Recruitment/RecruitmentData")]
public class RecruitmentData : ScriptableObject
{
    public string type;            // 가챠종류
    public string image;           // 가챠 종류에 맞는 사진
    public float aGradeRate;       // A등급 확률
    public float bGradeRate;       // B등급 확률
    public float cGradeRate;       // C등급 확률
}
