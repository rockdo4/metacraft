using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearUiController : View
{
    public ClearHeroInfo clearHeroPref;
    public Transform heroTr;
    public RewardManager rewards;

    public Button nodeButton;
    public Button lastNodeButton;
    public int baseExp = 0;


    private List<ClearHeroInfo> heroes = new();
    private void OnEnable()
    {        
        //AudioManager.Instance.ChageBGMOnlyFadeOut(9);
    }
    public void ResetUi()
    {
        baseExp = 0;
        rewards.ResetReward();
    }

    public void SetHeroes(List<AttackableUnit> _heroes)
    {
        foreach(var hero in _heroes)
        {
            var heroInfo = Instantiate(clearHeroPref, heroTr);
            heroInfo.SetInfo(hero);
            heroes.Add(heroInfo);
        }
    }

    public void SetData(bool isLast)
    {
        //플레이어 경험치 상승
        GameManager.Instance.AddOfficeExperience(baseExp);

        foreach (var hero in heroes)
        {
            int exp = baseExp;
            hero.expText.text = $"+{exp}";
            hero.Clear(exp);
        }

        rewards.SetReward(isLast);
    }
}
