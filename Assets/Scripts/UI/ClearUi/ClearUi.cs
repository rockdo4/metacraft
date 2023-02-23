using System.Collections.Generic;
using UnityEngine;

public class ClearUi : MonoBehaviour
{
    public ClearHeroInfo clearHeroPref;
    public Transform heroTr;
    public Rewards rewards;
    
    List<ClearHeroInfo> heroes = new();
    string expFormat = "+{0}";

    private void Awake()
    {
        //heros = heroTr.GetComponentsInChildren<ClearHeroInfo>();
        //foreach (var hero in heros)
        //{
        //    hero.SetInfo(1, 0); //실행될때 히어로 정보(레벨, 경험치 등 가져오기)
        //}
    }

    public void SetHoros(List<AttackableHero> heroes)
    {
        foreach(var hero in heroes)
        {
            var data = hero.GetHeroData();
            var heroInfo = Instantiate(clearHeroPref, heroTr);
            heroInfo.SetInfo(data.info.level, data.info.exp);

            this.heroes.Add(heroInfo);
        }
    }

    public void Clear()
    {
        var count = Random.Range(5, 10);

        foreach (var hero in heroes)
        {
            var exp = 400;
            hero.getExp.text = string.Format(expFormat, exp);
            hero.Clear(400);
        }

        rewards.SetReawrd(count);
    }
}
