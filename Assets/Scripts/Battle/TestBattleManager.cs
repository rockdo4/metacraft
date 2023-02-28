using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestBattleManager : MonoBehaviour
{
    public GameObject heroList;
    public AttackableHero cube;
    public HeroUi heroUi;
    public Transform heroUiTr;
    public List<Transform> startPositions;
    protected List<AttackableHero> useHeroes = new();
    public StageEnemy enemyCountTxt;

    public ClearUi clearUi;
    public List<MapEventTrigger> triggers;

    private void Awake()
    {
        //히어로 만들고, 히어로 ui만들고 서로 연결
        for (int i = 0; i < startPositions.Count; i++)
        {
            var hero = Instantiate(cube, startPositions[i].position, Quaternion.identity, heroList.transform);
            var heroNav = hero.GetComponent<NavMeshAgent>();
            heroNav.enabled = true;
            var heroUi = Instantiate(this.heroUi, heroUiTr);

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
