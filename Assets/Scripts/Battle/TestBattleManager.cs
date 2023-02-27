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
    public StageEnemy enemyCountTxt;

    public ClearUi clearUi;
    public List<MapEventTrigger> triggers;

    private void Awake()
    {
        //����� �����, ����� ui����� ���� ����

        for (int i = 0; i < startPositions.Count; i++)
        {
            var hero = Instantiate(cube, startPositions[i].position, Quaternion.identity, heroList.transform);
            var heroUi = Instantiate(battleHero, battleHeroTr);

            hero.SetUi(heroUi);
            heroUi.SetHeroInfo(hero.GetUnitData());
            useHeroes.Add(hero);
        }
        clearUi.SetHeroes(useHeroes);
    }

    public List<Transform> GetStartPosition()
    {
        return startPositions;
    }

    public int GetAllEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < triggers.Count; i++)
        {
            count += triggers[i].enemys.Count;
        }

        return count;
    }

    public void GetHeroList(ref List<AttackableHero> heroList)
    {
        heroList = useHeroes;
    }
    public virtual void OnDeadHero(AttackableHero hero)
    {
        useHeroes.Remove(hero);
    }
    public virtual void OnDeadEnemy(AttackableEnemy enemy)
    {
        enemyCountTxt.DimEnemy();
    }
    public virtual void GetEnemyList(ref List<AttackableEnemy> enemyList) { }
    public virtual void OnReady()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.MoveNext);
        }
    }
}
