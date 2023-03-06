using System.Collections.Generic;
using UnityEngine;

public class ClearUi : View
{
    public ClearHeroInfo clearHeroPref;
    public Transform heroTr;
    public Rewards rewards;
    
    private List<ClearHeroInfo> heroes = new();

    //private void Awake()
    //{
    //    //heros = heroTr.GetComponentsInChildren<ClearHeroInfo>();
    //    //foreach (var hero in heros)
    //    //{
    //    //    hero.SetInfo(1, 0); //실행될때 히어로 정보(레벨, 경험치 등 가져오기)
    //    //}
    //}

    public void SetHeroes(List<AttackableHero> _heroes)
    {
        foreach(var hero in _heroes)
        {
            var data = hero.GetUnitData().data;
            var heroInfo = Instantiate(clearHeroPref, heroTr);
            heroInfo.SetInfo(data.level, data.exp);

            heroes.Add(heroInfo);
        }
    }

    public void SetData()
    {
        var count = Random.Range(5, 10);

        foreach (var hero in heroes)
        {
            var exp = 400;
            hero.expText.text = $"+{exp}";
            hero.Clear(400);
        }

        rewards.SetReawrd(count);
    }
}
