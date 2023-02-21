using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattleManager : MonoBehaviour
{
    public AttackableHero cube;

    public BattleHero battleHero;
    public Transform battleHeroTr;

    public List<Transform> startPositions;

    private void Awake()
    {
        //����� �����, ����� ui����� ���� ����

        var heroes = Instantiate(new GameObject("Heroes"));
        for (int i = 0; i < startPositions.Count; i++)
        {
            var hero = Instantiate(cube, startPositions[i].position, Quaternion.identity, heroes.transform);
            var heroUi = Instantiate(battleHero, battleHeroTr);

            hero.SetUi(heroUi);
        }
    }
}
