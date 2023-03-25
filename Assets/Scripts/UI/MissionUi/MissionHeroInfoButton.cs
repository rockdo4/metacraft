public class MissionHeroInfoButton : HeroInfoButton
{

    public override void SetData(CharacterDataBundle dataBundle)
    {
        base.SetData(dataBundle);
    }

    public override void OnClick()
    {
        base.OnClick();
        MissionManager mm = FindObjectOfType<MissionManager>();
        mm.OnClickHeroSelect(bundle);
    }
}