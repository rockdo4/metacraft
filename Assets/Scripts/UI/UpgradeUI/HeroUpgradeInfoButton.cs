
public class HeroUpgradeInfoButton : HeroInfoButton
{
    //public TextMeshProUGUI energyConsumption;
    //public Slider hpBar;

    public override void SetData(CharacterDataBundle dataBundle)
    {
        base.SetData(dataBundle);
        //data = dataBundle.data;
        //hpBar.maxValue = data.healthPoint;
        //hpBar.value = data.currentHp;
    }

    public override void OnClick()
    {
        base.OnClick();
    }
}