using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;
using Cinemachine;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviour
{
    [Header("사용할 맵들")]
    public List<GameObject> eventMaps;
    private List<Dictionary<string, object>> eventInfoTable;            // 이벤트 테이블
    private List<Dictionary<string, object>> supplyInfoTable;           // 보급 테이블
    private Dictionary<string, object> currentSelectMissionTable;       // 작전 테이블

    public MapEventEnum curEvent = MapEventEnum.Normal;
    private GameObject curMap;

    [Header("이벤트 Ui")]
    public GameObject eventUi;
    [Header("이벤트 발생 시 클릭할 버튼들")]
    public List<GameObject> choiceButtons;
    [Header("이벤트맵 히어로 이미지 Ui")]
    public Image battleEventHeroImage;
    [Header("이벤트 설명 들어갈 텍스트")]
    public TextMeshProUGUI contentText;

    [Header("보급 Ui")]
    public GameObject supplyUi;
    [Header("보급맵 발생 시 클릭할 버튼들")]
    public List<GameObject> supplyButtons;
    [Header("보급맵 히어로 이미지 Ui")]
    public List<HeroUi> supplyEventHeroImages;
    [Header("보급 설명 들어갈 텍스트")]
    public TextMeshProUGUI supplyContentText;

    private List<TextMeshProUGUI> buttonTexts = new();
    private List<TextMeshProUGUI> supplyButtonTexts = new();

    private List<string> heroNames = new();

    // TestBattleManager
    public List<HeroUi> heroUiList;
    public List<AttackableUnit> useHeroes = new();
    public List<AttackableUnit> unuseHeroes = new();
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
    public float platformMoveSpeed = 5f;
    public int currTriggerIndex = 0;
    private float nextStageMoveTimer = 0f;
    private Coroutine coMovingMap;
    private Coroutine coResetMap;

    private BattleMapInfo currBtMgr;
    private List<MapEventTrigger> btMapTriggers = new();

    private GameObject viewPoint;
    private Vector3 viewPointInitPos;

    public CinemachineVirtualCamera cinemachine;
    private int enemyTriggerIndex = 0;                          // 방어전에 쓰일것 (에너미 스폰하는 트리거)
    private List<Light> lights = new();

    private void Start()
    {
        Init();
        StartNextStage(curEvent);
    }

    private void CloseUi(GameObject ui, List<GameObject> buttons)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetActive(false);
        }
        SetEventUiActive(ui, false);
    }

    public void EndEvent()
    {
        CloseUi(eventUi, choiceButtons);
    }
    public void EndSupply()
    {
        CloseUi(supplyUi, supplyButtons);
        for (int i = 0; i < heroUiList.Count; i++)
        {
            heroUiList[i].gameObject.SetActive(true);
        }
    }

    private void StartNextStage(MapEventEnum ev)
    {
        curEvent = ev;

        SetStageEvent(ev);
        StartStage();
        if (currBtMgr.GetBattleMapType() == BattleMapEnum.Normal && curEvent == MapEventEnum.Normal)
        {
            for (int i = 0; i < useHeroes.Count; i++)
                Invoke(nameof(OnReady), 3f);
        }
    }

    private void OnLigth(int index)
    {
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].gameObject.SetActive(false);
        }

        lights[index].gameObject.SetActive(true);
    }

    private void SetStageEvent(MapEventEnum ev)
    {
        if (ev == MapEventEnum.Normal)
        {
            curMap = eventMaps[0];
            OnLigth(0);
        }
        else if (ev == MapEventEnum.Defense)
        {
            curMap = eventMaps[1];
            OnLigth(1);
        }
        else
        {
            curMap = eventMaps[2];
            OnLigth(2);

            if (tree.CurNode.type == TreeNodeTypes.Supply)
            {
                SetEventUiActive(supplyUi, true);
                SetActiveHeroUiList();
            }
            else
            {
                SetEventUiActive(eventUi, true);
            }

            SetEventInfo(ev);
        }
    }

    private void SetEventUiActive(GameObject ui, bool active) => ui.SetActive(active);

    private void SetActiveHeroUiList()
    {
        for (int i = 0; i < heroUiList.Count; i++)
        {
            heroUiList[i].gameObject.SetActive(false);
        }
    }

    private void SetEventInfo(MapEventEnum ev)
    {
        if (tree.CurNode.type == TreeNodeTypes.Supply)
        {
            // 작전 테이블에서 보급 id 찾고
            string supplyId = $"{currentSelectMissionTable["SupplyID"]}";

            // 보급 테이블에서 같은 ID 찾아서 해당 줄의 인덱스 저장
            int index = 0;
            for (int i = 0; i < supplyInfoTable.Count; i++)
            {
                if (supplyInfoTable[i][supplyId].Equals(supplyId))
                {
                    index = i;
                    break;
                }
            }

            // 찾은 인덱스로 해당 줄의 데이터들을 불러옴
            supplyContentText.text = $"{supplyInfoTable[index]["supply_text"]}";
            for (int i = 0; i < supplyButtons.Count; i++)
            {
                choiceButtons[i].SetActive(true);
                string text = $"{supplyInfoTable[index][$"choice{i}_text"]}";
                supplyButtonTexts[i].text = text;
            }
        }
        else
        {
            int heroNameIndex = Random.Range(0, heroNames.Count);
            battleEventHeroImage.sprite = GameManager.Instance.GetSpriteByAddress($"Icon_{heroNames[heroNameIndex]}");
            contentText.text = $"{eventInfoTable[(int)ev]["Eventtext"]}";

            int textCount = (int)eventInfoTable[(int)ev][$"TextCount"];
            for (int i = 0; i < textCount; i++)
            {
                choiceButtons[i].SetActive(true);
                string text = $"{eventInfoTable[(int)ev][$"Text{i + 1}"]}";
                buttonTexts[i].text = text;
            }
        }
    }

    private void Init()
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var text = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonTexts.Add(text);
        }
        for (int i = 0; i < supplyButtons.Count; i++)
        {
            var text = supplyButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            supplyButtonTexts.Add(text);
        }

        GameManager gm = GameManager.Instance;
        eventInfoTable = gm.eventInfoList;
        supplyInfoTable = gm.supplyInfoList;
        currentSelectMissionTable = gm.currentSelectMission;

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

                supplyEventHeroImages[i].SetHeroInfo(attackableHero.GetUnitData());
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

        gm.SetDifferentColor();
        for (int i = 0; i < eventMaps.Count; i++)
        {
            BattleMapInfo battleMap = eventMaps[i].GetComponent<BattleMapInfo>();
            Light battleMapLigth = battleMap.GetLight();
            battleMapLigth.color = gm.GetMapLightColor();
            battleMapLigth.gameObject.SetActive(false);
            lights.Add(battleMapLigth);
        }
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
        unuseHeroes.Add(hero);
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

        for (int i = 0; i < unuseHeroes.Count; i++)
        {
            Utils.CopyPositionAndRotation(unuseHeroes[i].gameObject, GameManager.Instance.heroSpawnTransform);
            unuseHeroes[i].ResetData();
            unuseHeroes[i].SetMaxHp();
            unuseHeroes[i].SetEnabledPathFind(false);
            unuseHeroes[i].gameObject.SetActive(false);
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
        yield return new WaitForSeconds(nextStageMoveTimer);

        float curMaxZPos = viewPoint.transform.position.z;
        float nextMaxZPos = btMapTriggers[currTriggerIndex + 1].heroSettingPositions.Max(transform => transform.position.z);

        currTriggerIndex++;
        for (int i = 0; i < useHeroes.Count; i++)
        {
            var pos = btMapTriggers[currTriggerIndex].heroSettingPositions[i];
            useHeroes[i].MoveNext(pos.transform.position);
        }

        // 플랫폼 무브 스피드 히어로 무브 스피드로 바꾸기
        while (viewPoint.transform.position.z <= nextMaxZPos)
        {
            viewPoint.transform.Translate(Vector3.forward * platformMoveSpeed * Time.deltaTime);
            yield return null;
        }

        if (!btMapTriggers[currTriggerIndex].isMissionEnd)
        {
            if (btMapTriggers[currTriggerIndex].isSkip)
            {
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

    private IEnumerator CoResetMap(float timer)
    {
        yield return new WaitForSeconds(timer);

        if (OnNextStage())
        {
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
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.Idle);
        }

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
            choiceButtons[i].SetActive(true);
            //roadChoiceButtons[i].choiceIndex = i;
        }
    }
    private void CreateRoad()
    {
        if (tree.CurNode == null)
            tree.CreateTreeGraph();

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
        viewPoint = currBtMgr.GetViewPoint();
        viewPointInitPos = viewPoint.transform.position;
        cinemachine.Follow = viewPoint.transform;

        CreateRoad();
        AddRoadTrigger();

        List<Transform> startPositions = currBtMgr.GetStartPosition();
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].gameObject.SetActive(true);
            Utils.CopyPositionAndRotation(useHeroes[i].gameObject, startPositions[i]);
            useHeroes[i].SetEnabledPathFind(true);
        }

        currBtMgr.GameStart();
    }

    private void EndStage() // 맵 꺼지기 직전에 실행
    {
        viewPoint.transform.position = viewPointInitPos;
        cinemachine.Follow = viewPoint.transform;
        DestroyRoad();
        RemoveRoadTrigger();
        ResetRoads();
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ResetData();
            useHeroes[i].SetEnabledPathFind(false);
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
            //StartNextStage(MapEventEnum.Defense);
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

        switch (currBtMgr.GetBattleMapType())
        {
            case BattleMapEnum.Normal:
                EnemyCountCheck(enemy, currTriggerIndex);
                break;
            case BattleMapEnum.Defense:
                EnemyCountCheck(enemy , enemyTriggerIndex);
                break;
        }
    }

    private void EnemyCountCheck(AttackableEnemy enemy ,int triggerIndex)
    {
        btMapTriggers[triggerIndex].OnDead(enemy);
        int count = btMapTriggers[triggerIndex].useEnemys.Count;
        if (count == 0)
        {
            SetHeroReturnPositioning(btMapTriggers[currTriggerIndex].heroSettingPositions);
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

    public void SetEnemyTriggerIndex(int index)
    {
        enemyTriggerIndex = index;
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
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 작전 테이블에서 보급 id 찾고
            if (currentSelectMissionTable == null)
                Logger.Debug("Null");
            string supplyId = $"{currentSelectMissionTable["SupplyID"]}";

            // 보급 테이블에서 같은 ID 찾아서 해당 줄의 인덱스 저장
            int index = 0;
            for (int i = 0; i < supplyInfoTable.Count; i++)
            {
                if (supplyInfoTable[i]["ID"].Equals(supplyId))
                {
                    index = i;
                    break;
                }
            }

            // 찾은 인덱스로 해당 줄의 데이터들을 불러옴
            supplyContentText.text = $"{supplyInfoTable[index]["supply_text"]}";
            for (int i = 0; i < supplyButtons.Count; i++)
            {
                choiceButtons[i].SetActive(true);
                string text = $"{supplyInfoTable[index][$"choice{i + 1}_text"]}";
                supplyButtonTexts[i].text = text;
            }
        }
    }

    /*********************************************  임시  **********************************************/
    // Attackable Hero 스크립트의 ReturnPosUpdate 함수 내에서 사용하고 있음
    // 여기 어케 바꿀지 생각해봐야함
    public bool TempReturnPos()
    {
        for (int i = 0; i < btMapTriggers[enemyTriggerIndex].enemySettingPositions.Count; i++)
        {
            if (!btMapTriggers[enemyTriggerIndex].enemySettingPositions[i].GetMiddleBossIsAlive())
            {
                Logger.Debug("Next!");
                //KillAllEnemy(enemyTriggerIndex);
                return true;
            }
        }

        return false;
    }
    private void KillAllEnemy(int index)
    {
        int count = btMapTriggers[index].useEnemys.Count;

        for (int i = 0; i < count; i++)
        {
            OnDeadEnemy((AttackableEnemy)btMapTriggers[index].useEnemys[i]);
        }
    }
}
