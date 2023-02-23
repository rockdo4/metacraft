using System.Collections.Generic;
using UnityEngine;

public class TestBattleManager : MonoBehaviour
{
    public GameObject heroList;
    public AttackableHero cube;
    public BattleHero battleHero;
    public Transform battleHeroTr;
    public List<Transform> startPositions;
    protected List<AttackableHero> useHeroes = new();

    public ClearUi clearUi;

    private void Awake()
    {
        //히어로 만들고, 히어로 ui만들고 서로 연결

        for (int i = 0; i < startPositions.Count; i++)
        {
            var hero = Instantiate(cube, startPositions[i].position, Quaternion.identity, heroList.transform);
            var heroUi = Instantiate(battleHero, battleHeroTr);

            hero.SetUi(heroUi);
            useHeroes.Add(hero);
            heroUi.SetHeroInfo(hero.GetHeroData());
        }
        clearUi.SetHoros(useHeroes);
    }

    public List<Transform> GetStartPosition()
    {
        return startPositions;
    }
}
