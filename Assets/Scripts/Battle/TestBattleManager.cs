using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestBattleManager : MonoBehaviour
{
    public GameObject heroList;
    public AttackableHero heroPref;
    public List<HeroUi> heroUiList;
    public List<Transform> startPositions;
    public List<AttackableHero> useHeroes = new();
    public StageEnemy enemyCountTxt;

    public ClearUi clearUi;
    public List<MapEventTrigger> triggers;

    protected int readyCount;

    private void Awake()
    {

        int index = 0;
        List<GameObject> selectedHeroes = GameManager.Instance.GetSelectedHeroes();
        int notNullCount = 0;
        foreach (GameObject hero in selectedHeroes)
            if (hero != null) notNullCount++;

        if (notNullCount == 0)
        {
            //히어로 만들고, 히어로 ui만들고 서로 연결
            for (int i = 0; i < startPositions.Count; i++)
            {
                var hero = Instantiate(heroPref, startPositions[i].position, Quaternion.identity, heroList.transform);
                var heroNav = hero.GetComponent<NavMeshAgent>();
                heroNav.enabled = true;
                heroUiList[i].gameObject.SetActive(true);

                hero.SetUi(heroUiList[i]);
                heroUiList[i].SetHeroInfo(hero.GetUnitData());
                useHeroes.Add(hero);
            }
        }
        else
        {
            foreach (GameObject hero in selectedHeroes)
            {
                if (hero != null)
                {
                    hero.SetActive(true);
                    Utils.CopyTransform(hero, startPositions[index]);
                    NavMeshAgent heroNav = hero.GetComponent<NavMeshAgent>();
                    heroNav.enabled = true;
                    AttackableHero attackableHero = hero.GetComponent<AttackableHero>();
                    attackableHero.SetUi(heroUiList[index]);
                    heroUiList[index].SetHeroInfo(attackableHero.GetUnitData());
                    heroUiList[index].gameObject.SetActive(true);
                    useHeroes.Add(attackableHero);
                }
                index++;
            }
        }

        clearUi.SetHeroes(useHeroes);

        readyCount = useHeroes.Count;
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
    public void EnemyTriggerEnter()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.Battle);
        }
    }
    protected virtual void SetHeroReturnPositioning(List<Transform> pos)
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].SetReturnPos(pos[i]);
            useHeroes[i].ChangeUnitState(UnitState.ReturnPosition);
        }
    }
    // 클리어 시 호출할 함수 (Ui 업데이트)
    protected void SetStageClear()
    {
        UIManager.Instance.ShowView(1);
        clearUi.SetData();
        Logger.Debug("Clear!");
    }
}
