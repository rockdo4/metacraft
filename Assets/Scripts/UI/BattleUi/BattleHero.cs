using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleHero : MonoBehaviour
{
    public HeroSkill heroSkill;

    [SerializeField]
    private BuffList viewBuffList;
    private List<HeroBuff> buffList = new();
    public HeroState heroState;

    private void Awake()
    {
        viewBuffList.SetList(ref buffList);
    }

    public void AddBuff()
    {
        viewBuffList.AddBuff();
    }

    public void OnClickHeroSkill()
    {
        if(heroState == HeroState.Battle)
            heroSkill.OnClickSkill();
    }

    public void OnClickPopUp()
    {
        if (heroState == HeroState.Battle)
            viewBuffList.OnClickPopUp();
    }
}
