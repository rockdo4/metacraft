using System.Collections.Generic;
using UnityEngine;

public class ClearUiController : View
{
    public ClearHeroInfo clearHeroPref;
    public Transform heroTr;
    public RewardManager rewards;
    
    private List<ClearHeroInfo> heroes = new();

    public void SetHeroes(List<AttackableUnit> _heroes)
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
        //플레이어 경험치 상승
        int difficulty = (int)GameManager.Instance.currentSelectMission["Difficulty"];
        int baseExp = 100;
        GameManager.Instance.AddOfficeExperience(baseExp * difficulty);

        foreach (var hero in heroes)
        {
            int exp = baseExp * difficulty;
            hero.expText.text = $"+{exp}";
            hero.Clear(exp);
        }

        rewards.SetReward();
    }
}
