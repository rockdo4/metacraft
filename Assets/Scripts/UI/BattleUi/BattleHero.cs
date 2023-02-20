using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleHero : MonoBehaviour
{
    public HeroSkill heroSkill;

    [SerializeField]
    private BuffList viewBuffList;
    private List<HeroBuff> buffList = new();

    private void Awake()
    {
        viewBuffList.SetList(ref buffList);
    }

    public void AddBuff()
    {
        viewBuffList.AddBuff();
    }

    public void OnClickHeroSkill() => heroSkill.OnClickSkill();
    public void SkillEffect()
    {
        viewBuffList.AddBuff();
    }
    public void OnClickPopUp() => viewBuffList.OnClickPopUp();
}
