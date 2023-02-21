using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattleManager : MonoBehaviour
{
    public AttackableHero cube;

    public BattleHero battleHero;
    public Transform battleHeroTr;

    private void Awake()
    {
        //히어로 만들고, 히어로 ui만들고 서로 연결
        var hero = Instantiate(cube);
        var heroUi = Instantiate(battleHero, battleHeroTr);

        hero.SetUi(heroUi);
    }
}
