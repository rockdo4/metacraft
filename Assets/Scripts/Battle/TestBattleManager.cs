using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TreeEditor;
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

    public TreeMapSystem tree;
    
    // Test Member
    public List<GameObject> roadPrefab;
    protected List<ForkedRoad> roads = new();
    protected GameObject road;
    public Transform roadTr;
    protected Coroutine coFadeIn;
    protected Coroutine coFadeOut;
    public List<RoadChoiceButton> choiceButtons;
    protected List<TextMeshProUGUI> choiceButtonTexts = new();
    protected int nodeIndex;
    private EventManager evManager;

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
        foreach(var hero in useHeroes)
        {
            hero.PassiveSkillEvent();
        }
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var text = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            choiceButtonTexts.Add(text);
        }

        clearUi.SetHeroes(useHeroes);
        readyCount = useHeroes.Count;

        FindObjectOfType<AutoButton>().ResetData();
        evManager = FindObjectOfType<EventManager>();
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
            // Logger.Debug("Return");
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
        // Logger.Debug("Clear!");
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

    protected void StartFadeOut()
    {
        coFadeOut = StartCoroutine(CoFadeOut());
    }

    public virtual void SelectNextStage(int index)
    {
        //int stageIndex = nodeIndex = index; // choiceButtons[index].choiceIndex;
        //thisNode = thisNode.childrens[stageIndex];

        nodeIndex = index;
        TreeNodeObject prevNode = tree.CurNode;
        tree.CurNode = prevNode.childrens[index];
        readyCount = useHeroes.Count;
        int childCount = prevNode.childrens.Count;
        for (int i = 0; i< childCount; i++)
        {
            prevNode.childrens[i].nodeButton.onClick.RemoveAllListeners();
        }
        tree.OffMovableHighlighters();

        //for (int i = 0; i < choiceButtons.Count; i++)
        //{
        //    choiceButtons[i].gameObject.SetActive(false);
        //}
    }

    protected void ChoiceNextStageByNode()
    {
        tree.gameObject.SetActive(true);

        List<TreeNodeObject> childs = tree.CurNode.childrens;
        tree.SetMovableHighlighter(tree.CurNode);
        int count = childs.Count;
        for (int i = 0; i < count; i++)
        {
            int num = i;
            childs[i].nodeButton.onClick.AddListener(() => SelectNextStage(num));
        }
    }

    protected void ChoiceNextStage()
    {
        TreeNodeObject thisNode = tree.CurNode;
        int count = thisNode.childrens.Count;
        for (int i = 0; i < count; i++)
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
            useHeroes[i].SetMaxHp();
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
        // Logger.Debug("Fail!");
    }

    // 길목 생성
    protected void CreateRoad(GameObject platform)
    {
        TreeNodeObject thisNode = tree.CurNode;
        if (thisNode.childrens.Count == 0)
        {
            return;
        }

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
        {
            return;
        }
         

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
        tree.gameObject.SetActive(false);
    }

    protected void OnStageComplete()
    {
        evManager.EndEvent();
    }
    protected bool OnNextEvent()
    {
        if (tree.CurNode.type == TreeNodeTypes.Event)
        {
            var randomEvent = Random.Range((int)MapEventEnum.CivilianRescue, (int)MapEventEnum.Count);
            evManager.StartEvent((MapEventEnum)randomEvent);
            return true;
        }

        return false;
    }
}
