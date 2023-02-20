using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleHero : MonoBehaviour
{
    public HeroSkill heroSkill;
    public HeroAttack heroAttack;

    [SerializeField]
    private BuffList viewBuffList;
    public List<HeroBuff> buffList = new();
    CharactorData charactorData;

    private void Awake()
    {
        viewBuffList.SetList(ref buffList);


        //Test
        SetData(SetTestData());
    }

    public void AddBuff()
    {
        viewBuffList.AddBuff();
    }

    public void SetData(CharactorData data)
    {
        charactorData = data; //데이터 갱신

        heroSkill.Set(charactorData.skillCooldown, data.skillEvent); //스킬 쿨타임, 이벤트 세팅
        heroAttack.Set(charactorData.cooldown, data.attackEvent);
    }

    public void OnClickHeroSkill() => heroSkill.OnClickSkill();
    public void SkillEffect()
    {
        viewBuffList.AddBuff();
    }
    public void OnClickPopUp() => viewBuffList.OnClickPopUp();


    public CharactorData SetTestData()
    {
        CharactorData testData = new CharactorData();

        testData.damage = 100;
        testData.def = 20;
        testData.hp = 100; 
        testData.speed = 20;
        testData.chritical = 50; 
        testData.chriticalDmg = 2;
        testData.evasion = 20;
        testData.accuracy = 80; 
        testData.grade = 'A'; 
        testData.level = 1; 
        testData.type = "TEST";
        testData.cooldown = 1;
        testData.skillCooldown = 3;
        testData.exp = 0;
        testData.attackEvent = () => {  };
        testData.skillEvent = () => {  AddBuff(); };
        testData.passiveEvent = () => {  };

        return testData;
    }

    public void Attack()
    {
        Logger.Debug("Attack Test");

    }
    public void Skill()
    {
        Logger.Debug("Skill Test");

    }
    public void Passive()
    {
        Logger.Debug("Passive Test");

    }
}
