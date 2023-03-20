using UnityEngine;

[CreateAssetMenu(fileName = "RecruitmentData", menuName = "Recruitment/RecruitmentData")]
public class RecruitmentData : ScriptableObject
{
    public string type;                // °¡Ã­Á¾·ù
    public int itemValue = 5;          // ÇÊ¿ä¾ÆÀÌÅÛ °¹¼ö
    public float aGradeRate;       // Aµî±Þ È®·ü
    public float bGradeRate;       // Bµî±Þ È®·ü
    public float cGradeRate;       // Cµî±Þ È®·ü
}
