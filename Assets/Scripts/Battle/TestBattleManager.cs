using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TestBattleManager : MonoBehaviour
{
    public List<HeroUi> heroUiList;
    public List<Transform> startPositions;
    public List<AttackableUnit> useHeroes = new();
    public StageEnemy enemyCountTxt;

    public ClearUiController clearUi;
    public List<MapEventTrigger> triggers;

    protected int readyCount;

    public Image fadePanel;

    public GeneratePolynomialTreeMap tree;
    protected TreeNodeObject thisNode;

    // Test Member
    public List<GameObject> roadPrefab;
    protected List<ForkedRoad> roads;
    protected GameObject road;
    public Transform roadTr;
    protected Coroutine coFadeIn;
    protected Coroutine coFadeOut;
    public List<RoadChoiceButton> choiceButtons;
    protected List<TextMeshProUGUI> choiceButtonTexts = new();
    protected int nodeIndex;

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
        
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var text = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            choiceButtonTexts.Add(text);
        }

        tree.CreateTreeGraph();
        thisNode = tree.root; // 현재 위치한 노드

        // tree.root.type 맵 타입
        // tree.root.childrens 맵 순서
        // thisNode = tree.root.childrens[0]; 다음 노드 선택할 때 쓰는 것

        clearUi.SetHeroes(useHeroes);
        readyCount = useHeroes.Count;

        FindObjectOfType<AutoButton>().ResetData();
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

    public void GetHeroList(ref List<AttackableUnit> heroList)
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
    public virtual void GetEnemyList(ref List<AttackableUnit> enemyList) { }
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
            ((AttackableHero)useHeroes[i]).SetReturnPos(pos[i]);
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

    protected IEnumerator CoFadeIn()
    {
        fadePanel.gameObject.SetActive(true);
        float fadeAlpha = 0f;
        while (fadeAlpha < 1f)
        {
            fadeAlpha += 0.01f;
            yield return null;
            fadePanel.color = new Color(0, 0, 0, fadeAlpha);
        }
        yield break;
    }

    protected IEnumerator CoFadeOut()
    {
        float fadeAlpha = 1f;
        while (fadeAlpha > 0f)
        {
            fadeAlpha -= 0.01f;
            yield return null;
            fadePanel.color = new Color(0, 0, 0, fadeAlpha);
        }

        fadePanel.gameObject.SetActive(false);
        yield break;
    }

    public virtual void SelectNextStage(int index)
    {
        int stageIndex = nodeIndex = choiceButtons[index].choiceIndex;
        thisNode = thisNode.childrens[stageIndex];

        readyCount = useHeroes.Count;

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }
    }

    protected void ChoiceNextStage()
    {
        for (int i = 0; i < thisNode.childrens.Count; i++)
        {
            choiceButtonTexts[i].text = $"{thisNode.childrens[i].type}";
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].choiceIndex = i;
        }
    }

    public virtual void MoveNextStage(float timer)
    {
        coFadeIn = StartCoroutine(CoFadeIn());
    }

    // 히어로들 안 보이는 위치로 옮기고 Active False 시키는 함수
    public void ResetHeroes()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Utils.CopyPositionAndRotation(useHeroes[i].gameObject, GameManager.Instance.heroSpawnTransform);
            useHeroes[i].ResetData();
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
        GameManager.Instance.NextDay();
        UIManager.Instance.ShowView(2);
        Logger.Debug("Fail!");
    }

    // 길목 생성
    protected void CreateRoad(GameObject platform)
    {
        if (thisNode.childrens.Count == 0)
            return;

        road = Instantiate(roadPrefab[thisNode.childrens.Count - 1], platform.transform);
        road.transform.position = roadTr.transform.position;
        roads = road.GetComponentsInChildren<ForkedRoad>().ToList();
    }

    protected void DestroyRoad()
    {
        Destroy(road);
    }

    protected void AddRoadTrigger()
    {
        if (roads == null)
            return;

        for (int i = 0; i < roads.Count; i++)
        {
            triggers.Add(roads[i].fadeTrigger);
        }
    }

    protected void RemoveRoadTrigger()
    {
        for (int i = 0; i < roads.Count; i++)
        {
            triggers.Remove(roads[i].fadeTrigger);
        }
    }

    protected void ResetRoads()
    {
        roads.Clear();
        roads = null;
    }

    protected void ResetStage()
    {
        for (int i = 0; i < triggers.Count; i++)
        {
            triggers[i].ResetEnemys();
        }
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Utils.CopyPositionAndRotation(useHeroes[i].gameObject, startPositions[i]);
        }
    }
}
