using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MissionHeroInfoButton : HeroInfoButton
{
    public TextMeshProUGUI energyConsumption;
    public Slider hpBar;

    public override void SetData(CharacterDataBundle dataBundle)
    {
        base.SetData(dataBundle);
        data = dataBundle.data;
        energyConsumption.text = data.energy.ToString();
        hpBar.maxValue = data.healthPoint;
    }

    public void OnClick()
    {
        Logger.Debug(1);
        GameObject.FindObjectOfType<MissionManager>().OnClickHeroSelect(data);
    }
}