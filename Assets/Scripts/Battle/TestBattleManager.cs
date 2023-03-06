using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TestBattleManager : MonoBehaviour
{
    public GameObject heroList;
    public List<HeroUi> heroUiList;
    public List<Transform> startPositions;
    public List<AttackableHero> useHeroes = new();
    public StageEnemy enemyCountTxt;

    public ClearUiController clearUi;
    public List<MapEventTrigger> triggers;

    protected int readyCount;

    public Image fadePanel;
    public bool isFadeIn = true;

    private void Awake()
    {
        List<GameObject> selectedHeroes = GameManager.Instance.GetSelectedHeroes();
        int count = selectedHeroes.Count;

        for (int i = 0; i < count; i++)
        {
            if (selectedHeroes[i] != null)
            {
                selectedHeroes[i].SetActive(true);
                Utils.CopyPositionAndRotation(selectedHeroes[i], startPositions[i]);
                NavMeshAgent heroNav = selectedHeroes[i].GetComponent<NavMeshAgent>();
                heroNav.enabled = true;
                AttackableHero attackableHero = selectedHeroes[i].GetComponent<AttackableHero>();
                attackableHero.SetBattleManager(this);
                attackableHero.SetUi(heroUiList[i]);
                attackableHero.ResetData();
                heroUiList[i].SetHeroInfo(attackableHero.GetUnitData());
                heroUiList[i].gameObject.SetActive(true);
                useHeroes.Add(attackableHero);
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
        readyCount = useHeroes.Count;
        if (useHeroes.Count == 0)
        {
            SetStageFail();
        }
    }
    public virtual void OnDeadEnemy(AttackableEnemy enemy)
    {
        enemyCountTxt.DieEnemy();
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
            Logger.Debug("Return");
            useHeroes[i].SetReturnPos(pos[i]);
            useHeroes[i].ChangeUnitState(UnitState.ReturnPosition);
        }
    }
    // 클리어 시 호출할 함수 (Ui 업데이트)
    protected void SetStageClear()
    {
        UIManager.Instance.ShowView(1);
        GameManager.Instance.NextDay();
        clearUi.SetData();
        Logger.Debug("Clear!");
    }
    private IEnumerator CoFade()
    {
        if (isFadeIn)
        {
            float fadeAlpha = 0f;
            while (fadeAlpha < 1f)
            {
                fadeAlpha += 0.01f;
                yield return null;
                fadePanel.color = new Color(0, 0, 0, fadeAlpha);
            }

            isFadeIn = false;
            yield break;
        }
        else
        {
            float fadeAlpha = 1f;
            while (fadeAlpha > 0f)
            {
                fadeAlpha -= 0.01f;
                yield return null;
                fadePanel.color = new Color(0, 0, 0, fadeAlpha);
            }

            isFadeIn = true;
            yield break;
        }
    }
    protected void MoveNextStage()
    {
        StartCoroutine(CoFade());
    }

    // 히어로들 안 보이는 위치로 옮기고 Active False 시키는 함수
    public void ResetHeroes()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Utils.CopyPositionAndRotation(useHeroes[i].gameObject, GameManager.Instance.heroSpawnTransform);
            useHeroes[i].SetEnabledPathFind(false);
            useHeroes[i].gameObject.SetActive(false);
        }
    }

    public void OnDestroy()
    {
        Time.timeScale = 1;
    }

    protected void SetStageFail()
    {
        Time.timeScale = 0;
        UIManager.Instance.ShowView(2);
        Logger.Debug("Fail!");
    }
}
