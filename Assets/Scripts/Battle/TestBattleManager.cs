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
        for (int i = 0; i < 3; i++)
        {
            //����� �����, ����� ui����� ���� ����
            var hero = Instantiate(cube);
            var heroUi = Instantiate(battleHero, battleHeroTr);

            hero.SetUi(heroUi);
        }
    }
}
