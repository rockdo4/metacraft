using TMPro;
using UnityEngine;

public class HeroInfo : MonoBehaviour
{
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI levelText;

    private CharacterData baseData;

    public void SetData(CharacterData data)
    {
        baseData = data;
        heroNameText.text = baseData.heroName;
        gradeText.text = baseData.grade;
        typeText.text = baseData.type;
        levelText.text = baseData.level;
    }
}