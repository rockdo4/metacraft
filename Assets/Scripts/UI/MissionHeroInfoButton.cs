using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MissionHeroInfoButton : HeroInfoButton
{
    public TextMeshProUGUI energyConsumption;
    public Slider hpBar;

    public override void SetData(CharacterDataBundle data)
    {
        base.SetData(data);

    }
}