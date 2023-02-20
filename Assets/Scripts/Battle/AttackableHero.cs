using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableHero : MonoBehaviour
{
    protected CharactorData charactorData;

    protected AttackCoolDown attackCoolDown;
    protected FindTarget findTarget;

    protected BattleHero heroUi;

    private void Awake()
    {
        attackCoolDown = GetComponent<AttackCoolDown>();
        findTarget = GetComponent<FindTarget>();

        charactorData = SetTestData(); //임시 데이터 로드
    }

    // Ui와 연결, Ui에 스킬 쿨타임 연결
    public virtual void Set(BattleHero heroUI)
    {
        this.heroUi = heroUI;

        attackCoolDown.Set(charactorData.cooldown, Attack); //어택 함수 등록
        this.heroUi.heroSkill.Set(charactorData.skillCooldown, Skill); //스킬 함수 등록
    }

    public virtual void Attack()
    {
        Logger.Debug("Attack");
    }
    public virtual void Skill()
    {
        Logger.Debug("Skill");
    }

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

        testData.attackDistance = 1f;
        testData.attackCount = 1;

        testData.grade = 'A';
        testData.level = 1;
        testData.type = "TEST";
        testData.cooldown = 1;
        testData.skillCooldown = 0.3f;
        testData.exp = 0;

        return testData;
    }
}
