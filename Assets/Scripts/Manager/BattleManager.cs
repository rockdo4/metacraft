using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    [Header("사용할 맵들")]
    public List<GameObject> eventMaps;
    private List<Dictionary<string, object>> eventInfoTable;

    private MapEventEnum curEvent = MapEventEnum.Normal;
    private GameObject curMap;

    public GameObject eventUi;
    [Header("이벤트 발생 시 클릭할 버튼들")]
    public List<GameObject> choiceButtons;
    [Header("히어로 이미지 Ui")]
    public Image heroImage;
    [Header("이벤트 설명 들어갈 텍스트")]
    public TextMeshProUGUI contentText;

    private List<TextMeshProUGUI> buttonTexts = new();

    private List<string> heroNames = new();

    // TestBattleManager
    public List<HeroUi> heroUiList;
    private List<AttackableUnit> useHeroes = new();
    public StageEnemy enemyCountTxt;
    public ClearUiController clearUi;
    public Image fadePanel;
    public TreeMapSystem tree;
    private int nodeIndex;
    public List<RoadChoiceButton> roadChoiceButtons;
    private List<TextMeshProUGUI> choiceButtonTexts = new();
    private Coroutine coFadeIn;
    private Coroutine coFadeOut;
    private int readyCount;
    public List<GameObject> roadPrefab;
    private List<ForkedRoad> roads = new();
    private GameObject road;

    // BeltScrollManager
    private GameObject platform;
    public float platformMoveSpeed = 10f;
    private int currTriggerIndex = 0;
    private float nextStageMoveTimer = 0f;
    private Coroutine coMovingMap;
    private Coroutine coResetMap;

    private BattleMapInfo currBtMgr;
    private List<MapEventTrigger> btMapTriggers = new();


    private void Awake()
    {
        Init();
        StartNextStage(curEvent);
    }

    public void EndEvent()
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        SetEventUiActive(false);
    }

    private void SetActiveCurrMap(bool active)
    {
        curMap.SetActive(active);
    }

    private void StartNextStage(MapEventEnum ev)
    {
        curEvent = ev;

        if (curMap != null)
            SetActiveCurrMap(false);

        SetStageEvent(ev);
        StartStage();
        if (currBtMgr.GetBattleMapType() == BattleMapEnum.BeltScroll && curEvent == MapEventEnum.Normal)
        {
            for (int i = 0; i < useHeroes.Count; i++)
                Invoke(nameof(OnReady), 1f);
        }
    }

    private void SetStageEvent(MapEventEnum ev)
    {
        if (ev == MapEventEnum.Normal)
        {
            curMap = eventMaps[0];
        }
        else if (ev == MapEventEnum.Defense)
        {
            curMap = eventMaps[0];
        }
        else
        {
            curMap = eventMaps[2];
            SetEventInfo(ev);
            SetEventUiActive(true);
        }
    }

    private void SetEventUiActive(bool active) => eventUi.SetActive(active);

    private void SetEventInfo(MapEventEnum ev)
    {
        int heroNameIndex = Random.Range(0, heroNames.Count);
        heroImage.sprite = GameManager.Instance.GetSpriteByAddress($"Icon_{heroNames[heroNameIndex]}");
        contentText.text = $"{eventInfoTable[(int)ev]["Explanation"]}";

        int textCount = (int)eventInfoTable[(int)ev][$"TextCount"];
        for (int i = 0; i < textCount; i++)
        {
            choiceButtons[i].SetActive(true);
            string text = $"{eventInfoTable[(int)ev][$"Select{i + 1}Text"]}";
            buttonTexts[i].text = text;
        }
    }

    private void Init()
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var text = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonTexts.Add(text);
        }

        GameManager gm = GameManager.Instance;
        eventInfoTable = gm.eventInfoList;

        var selectedHeroes = gm.GetSelectedHeroes();
        int count = selectedHeroes.Count;

        for (int i = 0; i < count; i++)
        {
            if (selectedHeroes[i] != null)
            {
                AttackableHero attackableHero = selectedHeroes[i].GetComponent<AttackableHero>();
                attackableHero.SetBattleManager(this);
                attackableHero.SetUi(heroUiList[i]);
                attackableHero.ResetData();
                heroUiList[i].SetHeroInfo(attackableHero.GetUnitData());
                heroUiList[i].gameObject.SetActive(true);
                useHeroes.Add(attackableHero);
            }
        }
        for (int i = 0; i < useHeroes.Count; i++)
        {
            heroNames.Add(useHeroes[i].name);
            useHeroes[i].PassiveSkillEvent();
        }
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var text = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            choiceButtonTexts.Add(text);
        }

        clearUi.SetHeroes(useHeroes);
        readyCount = useHeroes.Count;

        FindObjectOfType<AutoButton>().ResetData();
    }

    private IEnumerator CoFadeIn()
    {
        fadePanel.gameObject.SetActive(true);
        float fadeAlpha = 0f;
        while (fadeAlpha < 1f)
        {
            fadeAlpha += Time.deltaTime;
            yield return null;
            fadePanel.color = new Color(0, 0, 0, fadeAlpha);
        }

        EndStage();
    }

    private IEnumerator CoFadeOut()
    {
        float fadeAlpha = 1f;
        while (fadeAlpha > 0f)
        {
            fadeAlpha -= Time.deltaTime;
            yield return null;
            fadePanel.color = new Color(0, 0, 0, fadeAlpha);
        }

        fadePanel.gameObject.SetActive(false);
    }

    public void OnReady()
    {
        readyCount--;

        if (readyCount == 0)
        {
            if (btMapTriggers[currTriggerIndex].isMissionEnd)
            {
                MissionClear();
            }
            else if (btMapTriggers[currTriggerIndex + 1] != null && btMapTriggers[currTriggerIndex + 1].isStageEnd)
            {
                ChoiceNextStageByNode();
            }
            else if (!btMapTriggers[currTriggerIndex].isStageEnd)
            {
                readyCount = useHeroes.Count;
                for (int i = 0; i < useHeroes.Count; i++)
                {
                    useHeroes[i].ChangeUnitState(UnitState.MoveNext);
                }
                coMovingMap = StartCoroutine(CoMovingMap());
            }
        }
    }

    private void SelectNextStage(int index)
    {
        nodeIndex = index;
        TreeNodeObject prevNode = tree.CurNode;
        tree.CurNode = prevNode.childrens[index];
        readyCount = useHeroes.Count;
        int childCount = prevNode.childrens.Count;

        for (int i = 0; i < childCount; i++)
        {
            prevNode.childrens[i].nodeButton.onClick.RemoveAllListeners();
        }
        tree.OffMovableHighlighters();

        SetHeroReturnPositioning(roads[nodeIndex].fadeTrigger.heroSettingPositions);
    }
    private void SetHeroReturnPositioning(List<Transform> pos)
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            ((AttackableHero)useHeroes[i]).SetReturnPos(pos[i]);
            useHeroes[i].ChangeUnitState(UnitState.ReturnPosition);
        }
    }
    private void MissionClear()
    {
        UIManager.Instance.ShowView(1);
        GameManager.Instance.NextDay();
        clearUi.SetData();
    }

    public void OnDeadHero(AttackableHero hero)
    {
        useHeroes.Remove(hero);
        readyCount = useHeroes.Count;
        if (useHeroes.Count == 0)
        {
            MissionFail();
        }
    }
    private void MissionFail()
    {
        Time.timeScale = 0;
        GameManager.Instance.NextDay();
        UIManager.Instance.ShowView(2);
    }
    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
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
    public void MoveNextStage(float timer)
    {
        StopCoroutine(coMovingMap);
        coFadeIn = StartCoroutine(CoFadeIn());
        coResetMap = StartCoroutine(CoResetMap(timer));
    }
    private IEnumerator CoMovingMap()
    {
        float curMaxZPos = 0f;
        float nextMaxZPos = 0f;
        float movePos = 0f;

        yield return new WaitForSeconds(nextStageMoveTimer);

        curMaxZPos = platform.transform.position.z +
            btMapTriggers[currTriggerIndex].heroSettingPositions.Max(transform => transform.position.z);

        if (!btMapTriggers[currTriggerIndex + 1].isStageEnd)
        {
            nextMaxZPos = btMapTriggers[currTriggerIndex + 1].heroSettingPositions.Max(transform => transform.position.z);
            movePos = curMaxZPos - nextMaxZPos;
            while (platform.transform.position.z >= movePos)
            {
                platform.transform.Translate((Vector3.forward * platformMoveSpeed * Time.deltaTime) * -1);
                yield return null;
            }

            currTriggerIndex++;


            if (!btMapTriggers[currTriggerIndex].isMissionEnd)
            {
                for (int i = 0; i < useHeroes.Count; i++)
                {
                    useHeroes[i].ChangeUnitState(UnitState.Battle);
                }

                if (btMapTriggers[currTriggerIndex].useEnemys.Count == 0 && currTriggerIndex != 0)
                {
                    Logger.Debug("NextTrigger");
                    ChoiceNextStageByNode();
                }
            }
            else
            {
                for (int i = 0; i < useHeroes.Count; i++)
                {
                    useHeroes[i].ChangeUnitState(UnitState.Idle);
                    OnReady();
                }
            }
        }
    }

    private IEnumerator CoResetMap(float timer)
    {
        yield return new WaitForSeconds(timer);

        if (OnNextStage())
        {
            Logger.Debug("OnNextStage");
            yield break;
        }

        if (tree.CurNode.type == TreeNodeTypes.Boss)
        {
            btMapTriggers.Last().isMissionEnd = true;
        }

        yield break;
    }
    public void GetHeroList(ref List<AttackableUnit> heroList)
    {
        heroList = useHeroes;
    }
    private void ChoiceNextStageByNode()
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
    private void ChoiceNextStage()
    {
        TreeNodeObject thisNode = tree.CurNode;
        int count = thisNode.childrens.Count;
        for (int i = 0; i < count; i++)
        {
            choiceButtonTexts[i].text = $"{thisNode.childrens[i].type}";
            choiceButtons[i].gameObject.SetActive(true);
            //roadChoiceButtons[i].choiceIndex = i;
        }
    }
    private void CreateRoad()
    {
        TreeNodeObject thisNode = tree.CurNode;
        if (thisNode.childrens.Count == 0)
        {
            return;
        }

        road = Instantiate(roadPrefab[thisNode.childrens.Count - 1], platform.transform);
        road.transform.position = currBtMgr.GetRoadTr().transform.position;
        roads = road.GetComponentsInChildren<ForkedRoad>().ToList();
    }
    private void DestroyRoad()
    {
        Destroy(road);
    }
    private void ResetRoads()
    {
        roads.Clear();
        roads = null;
    }
    private void StartStage()
    {
        currTriggerIndex = 0;

        currBtMgr = curMap.GetComponent<BattleMapInfo>();
        enemyCountTxt.Count = currBtMgr.GetAllEnemyCount();
        btMapTriggers = currBtMgr.GetTriggers();
        platform = currBtMgr.GetPlatform();
        platform.transform.position = Vector3.zero;

        SetActiveCurrMap(true);
        CreateRoad();
        AddRoadTrigger();

        List<Transform> startPositions = currBtMgr.GetStartPosition();
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].gameObject.SetActive(true);
            Utils.CopyPositionAndRotation(useHeroes[i].gameObject, startPositions[i]);
            useHeroes[i].SetEnabledPathFind(true);
        }

        for (int i = 0; i < btMapTriggers.Count; i++)
        {
            btMapTriggers[i].ResetEnemys();
        }
    }

    private void EndStage() // 맵 꺼지기 직전에 실행
    {
        DestroyRoad();
        RemoveRoadTrigger();
        ResetRoads();
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ResetData();
        }
    }
    private void RemoveRoadTrigger()
    {
        for (int i = 0; i < roads.Count; i++)
        {
            btMapTriggers.Remove(roads[i].fadeTrigger);
        }
    }
    private bool OnNextStage()
    {
        tree.gameObject.SetActive(false);

        coFadeOut = StartCoroutine(CoFadeOut());

        if (tree.CurNode.type == TreeNodeTypes.Event)
        {
            var randomEvent = Random.Range((int)MapEventEnum.CivilianRescue, (int)MapEventEnum.Count);
            StartNextStage((MapEventEnum)randomEvent);
            return true;
        }
        else
        {
            StartNextStage(MapEventEnum.Normal);
        }

        return false;
    }
    private void AddRoadTrigger()
    {
        if (roads == null)
        {
            return;
        }

        for (int i = 0; i < roads.Count; i++)
        {
            btMapTriggers.Add(roads[i].fadeTrigger);
        }
    }
    public int GetCurrTriggerIndex()
    {
        return currTriggerIndex;
    }

    public void SetHeroesReady()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            OnReady();
        }
    }

    public void OnDeadEnemy(AttackableEnemy enemy)
    {
        enemyCountTxt.DieEnemy();

        int count = 0;
        switch (currBtMgr.GetBattleMapType())
        {
            case BattleMapEnum.BeltScroll:
                btMapTriggers[currTriggerIndex].OnDead(enemy);
                count = btMapTriggers[currTriggerIndex].useEnemys.Count;

                if (count == 0)
                {
                    SetHeroReturnPositioning(btMapTriggers[currTriggerIndex].heroSettingPositions);
                }
                break;
            case BattleMapEnum.Defense:
                int index = 0;
                for (int i = 0; i < btMapTriggers.Count; i++)
                {
                    if (btMapTriggers[i].enemys.Count > 0)
                    {
                        index = i;
                        break;
                    }
                }

                btMapTriggers[index].OnDead(enemy);
                count = btMapTriggers[index].useEnemys.Count;

                if (count == 0)
                {
                    SetHeroReturnPositioning(btMapTriggers[currTriggerIndex].heroSettingPositions);
                }
                break;
        }
    }

    public void EnemyTriggerEnter()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.Battle);
        }
    }

    public BattleMapInfo GetCurrBtMgr()
    {
        return currBtMgr;
    }

    public void SetEnemyCountTxt(int count)
    {
        enemyCountTxt.Count = count;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            tree.CurNode.type = TreeNodeTypes.Boss;
            DestroyRoad();
            RemoveRoadTrigger();
            ResetRoads();
            btMapTriggers.Last().isMissionEnd = true;
        }
    }
}
