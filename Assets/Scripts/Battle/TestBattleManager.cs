using System.Collections.Generic;
using UnityEngine;

public class TestBattleManager : MonoBehaviour
{
    public GameObject heroList;
    public AttackableHero cube;
    public BattleHero battleHero;
    public Transform battleHeroTr;
    public List<Transform> startPositions;

    private void Awake()
    {
        //����� �����, ����� ui����� ���� ����

        for (int i = 0; i < startPositions.Count; i++)
        {
            var hero = Instantiate(cube, startPositions[i].position, Quaternion.identity, heroList.transform);
            var heroUi = Instantiate(battleHero, battleHeroTr);

            hero.SetUi(heroUi);
        }
    }

    public List<Transform> GetStartPosition()
    {
        return startPositions;
    }
}
