using System.Collections.Generic;
using UnityEngine;

public class ClearUiController : View
{
    public ClearHeroInfo clearHeroPref;
    public Transform heroTr;
    public RewardManager rewards;
    
    private List<ClearHeroInfo> heroes = new();

    public void SetHeroes(List<AttackableHero> _heroes)
    {
        foreach(var hero in _heroes)
        {
            var heroInfo = Instantiate(clearHeroPref, heroTr);
            heroInfo.SetInfo(hero);
            heroes.Add(heroInfo);
        }
    }

    public void SetData()
    {
        foreach (var hero in heroes)
        {
            int exp = (int)GameManager.Instance.currentSelectMission["Experience"];
            hero.expText.text = $"+{exp}";
            hero.Clear(exp);
        }

        rewards.SetReward();
    }
}
