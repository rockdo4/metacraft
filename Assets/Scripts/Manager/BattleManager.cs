using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;
using Cinemachine;

public class BattleManager : MonoBehaviour
{
    [Header("사용할 맵들")]
    public List<GameObject> eventMaps;
    private List<Dictionary<string, object>> eventInfoTable;            // 이벤트 테이블
    private List<Dictionary<string, object>> eventEffectInfoTable;      // 이벤트 이펙트 테이블
    private List<Dictionary<string, object>> supplyInfoTable;           // 보급 테이블
    private List<Dictionary<string, object>> enemyInfoTable;            // 적 스탯 테이블
    private List<Dictionary<string, object>> enemySpawnTable;           // 적 생성 테이블
    private Dictionary<string, object> currentSelectMissionTable;       // 작전 테이블

    public MapEventEnum curEvent = MapEventEnum.None;
    private GameObject curMap;

    [Header("이벤트 Ui")]
    public GameObject eventUi;
    [Header("이벤트 발생 시 클릭할 버튼들")]
    public List<Button> choiceButtons;
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

    [Header("스테이지 보상 Ui")]
    public StageReward stageReward;

    private List<TextMeshProUGUI> buttonTexts = new();
    private List<TextMeshProUGUI> supplyButtonTexts = new();

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
    private float platformMoveSpeed = 7f;
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
    public bool isMiddleBossAlive = true;
    private GameManager gm;

    public AttackableEnemy bossPrefab;
    public Button eventExitButton;

    //이벤트, 보급 등에서 사용하는 버프리스트
    public List<BuffInfo> buffList;

    [Header("생성할 적 프리펩들을 넣어주세요")]
    public List<AttackableEnemy> enemyPrefabs = new();

    private void Start()
    {
        Init();
        StartNextStage(curEvent);
    }

    private void SetActiveUi(GameObject ui, bool set) => ui.SetActive(set);
    private void SetActiveUi(Button ui, bool set) => ui.gameObject.SetActive(set);

    private void SetActiveUi(GameObject ui, List<GameObject> buttons, bool set, int buttonOnCount)
    {
        for (int i = 0; i < buttonOnCount; i++)
        {
            buttons[i].SetActive(set);
        }

        ui.SetActive(set);
    }
    private void SetActiveUi(GameObject ui, List<Button> buttons, bool set, int buttonOnCount)
    {
        for (int i = 0; i < buttonOnCount; i++)
        {
            buttons[i].gameObject.SetActive(set);
        }

        ui.SetActive(set);
    }
    public void EndEvent()
    {
        eventExitButton.gameObject.SetActive(false);
        SetActiveUi(eventUi, choiceButtons, false, choiceButtons.Count);
        SetHeroesReady();
    }
    public void EndSupply()
    {
        SetActiveUi(supplyUi, supplyButtons, false, supplyButtons.Count);
        for (int i = 0; i < heroUiList.Count; i++)
        {
            heroUiList[i].gameObject.SetActive(true);
        }
        NodeClearReward();
    }

    private void StartNextStage(MapEventEnum ev)
    {
        curEvent = ev;

        SetStageEvent(ev);
        StartStage();

        if (currBtMgr.GetBattleMapType() == BattleMapEnum.Normal && 
            (tree.CurNode.type == TreeNodeTypes.Normal || tree.CurNode.type == TreeNodeTypes.Root))
        {
            for (int i = 0; i < useHeroes.Count; i++)
                Invoke(nameof(OnReady), 3f);
        }
        else if (tree.CurNode.type == TreeNodeTypes.Villain)
        {
            // 보스맵에서 임시로 모두 OnReady 해줘서 움직이게함
            SetHeroesReady();
        }
    }

    private void SetStageEvent(MapEventEnum ev)
    {
        switch (tree.CurNode.type)
        {
            case TreeNodeTypes.Normal:
            case TreeNodeTypes.Root:
                curMap = eventMaps[0];
                break;
            case TreeNodeTypes.Threat:
                curMap = eventMaps[1];
                break;
            case TreeNodeTypes.Supply:
                curMap = eventMaps[2];
                for (int i = 0; i < supplyEventHeroImages.Count; i++)
                {
                    if (supplyEventHeroImages[i].heroData != null)
                        supplyEventHeroImages[i].SetHp();
                    else
                        supplyEventHeroImages[i].gameObject.SetActive(false);
                }
                SetActiveUi(supplyUi, supplyButtons, true, supplyButtons.Count);
                SetActiveHeroUiList(false);
                SetEventInfo(ev);
                break;
            case TreeNodeTypes.Event:
                curMap = eventMaps[2];
                SetActiveUi(eventUi, true);
                SetEventInfo(ev);
                break;
            case TreeNodeTypes.Villain:
                curMap = eventMaps[0];
                break;
        }

        DisabledAllMap();
        curMap.SetActive(true);
    }

    private void DisabledAllMap()
    {
        for (int i = 0; i < eventMaps.Count; i++)
        {
            eventMaps[i].SetActive(false);
        }
    }

    private void SetActiveHeroUiList(bool set)
    {
        for (int i = 0; i < heroUiList.Count; i++)
        {
            heroUiList[i].gameObject.SetActive(set);
        }
    }

    private void ExecutionBuff(int id)
    {
        var buff = FindBuff(id);

        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].AddValueBuff(buff);
        }
    }
    private BuffInfo FindBuff(int id)
    {
        foreach (var buff in buffList)
        {
            if (buff.id == id)
                return buff;
        }
        return null;
    }

    private void SetEventInfo(MapEventEnum ev)
    {
        if (tree.CurNode.type == TreeNodeTypes.Supply)
        {
            // 작전 테이블에서 보급 id 찾기
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

            string supplyTextId = $"{supplyInfoTable[index]["supply_text"]}";
            string findStringTable = gm.GetStringByTable(supplyTextId);
            supplyContentText.text = findStringTable;

            // 찾은 인덱스로 해당 줄의 데이터들을 불러옴
            for (int i = 0; i < supplyButtons.Count; i++)
            {
                supplyButtons[i].SetActive(true);

                string textId = $"{supplyInfoTable[index][$"choice{i + 1}_text"]}";

                string stringTableChoiceText = gm.GetStringByTable(textId);
                supplyButtonTexts[i].text = stringTableChoiceText;
            }
        }
        else
        {
            int heroNameIndex = Random.Range(0, useHeroes.Count);
            string heroName = useHeroes[heroNameIndex].GetUnitData().data.name;
            battleEventHeroImage.sprite = gm.GetSpriteByAddress($"Icon_{heroName}");
            string contentTextKey = $"{eventInfoTable[(int)ev]["Eventtext"]}";
            contentText.text = gm.GetStringByTable(contentTextKey);

            int textCount = (int)eventInfoTable[(int)ev][$"TextCount"];
            for (int i = 0; i < textCount; i++)
            {
                choiceButtons[i].gameObject.SetActive(true);
                string choiceTextKey = $"{eventInfoTable[(int)ev][$"Text{i + 1}"]}";
                string buttonText = gm.GetStringByTable(choiceTextKey);
                buttonTexts[i].text = buttonText;
            }
        }
    }

    public void OnClickEventChoiceButton(int index)
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }

        eventExitButton.gameObject.SetActive(true);
        SetEventEffectReward((int)curEvent, index + 1, contentText);
    }

    // 이벤트 노드 노멀 선택지 보상
    private void GetNormalEventEffect(ref string valueKey, ref int rewardKey, int effectColumn)
    {
        string normalV1Text = $"{eventEffectInfoTable[effectColumn]["Normalvalue1"]}";
        string normalV2Text = $"{eventEffectInfoTable[effectColumn]["Normalvalue2"]}";
        float normalValue1 = float.Parse(normalV1Text);
        float normalValue2 = float.Parse(normalV2Text);

        string value1Text = $"{eventEffectInfoTable[effectColumn]["NormalvalueText1"]}";
        string value2Text = $"{eventEffectInfoTable[effectColumn]["NormalvalueText2"]}";
        int normalReward1 = (int)eventEffectInfoTable[effectColumn]["NormalReward1"];
        int normalReward2 = (int)eventEffectInfoTable[effectColumn]["NormalReward2"];

        if (normalValue1.Equals(1f))
        {
            valueKey = value1Text;
            rewardKey = normalReward1;
        }
        else if (normalValue2.Equals(1f))
        {
            valueKey = value2Text;
            rewardKey = normalReward2;
        }
        else if (normalValue1.Equals(normalValue2))
        {
            float randomValue = Random.Range(0f, 1f);
            valueKey = randomValue >= 0.5f ? value1Text : value2Text;
            rewardKey = valueKey.Equals(value1Text) ? normalReward1 : normalReward2;
        }
        else
        {
            valueKey = normalValue1 > normalValue2 ? value1Text : value2Text;
            rewardKey = valueKey.Equals(value1Text) ? normalReward1 : normalReward2;
        }

        Logger.Debug($"normal value : {valueKey}, noraml reward : {rewardKey}");
    }

    private void GetPriorityTagEventEffect
        (ref string valueKey, ref int rewardKey, int effectColumn, List<string> tags, ref int rewardType)
    {
        // 태그를 찾았으면 해당 태그의 이벤트 불러오고 리턴
        for (int i = 0; i < useHeroes.Count; i++)
        {
            var heroTag = useHeroes[i].GetUnitData().data.tags;

            for (int j = 0; j < heroTag.Count; j++)
            {
                Logger.Debug($"hero tag : {heroTag[j]}");
                for (int k = 0; k < tags.Count; k++)
                {
                    if (heroTag[j].Equals(tags[k]))
                    {
                        // PriorityRewardType
                        valueKey = $"{eventEffectInfoTable[effectColumn][$"PriorityText{k + 1}"]}";
                        rewardKey = (int)eventEffectInfoTable[effectColumn][$"PriorityReward{k + 1}"];
                        rewardType = (int)eventEffectInfoTable[effectColumn][$"PriorityRewardType{k + 1}"];
                        Logger.Debug($"Get! [value : {valueKey}, reward : {rewardKey}]");
                        return;
                    }
                }
            }
        }

        // 못 찾았으면 노멀 이펙트로 이동해서 찾기
        GetNormalEventEffect(ref valueKey, ref rewardKey, effectColumn);
    }

    private void SetEventEffectReward(int column, int index, TextMeshProUGUI contentText)
    {
        string textEffect = $"{eventInfoTable[column][$"TextEffect{index}"]}";

        // eventEffectInfoList 는 eventEffectTagInfoList로 변경됨. 상운과 논의 후 연결해서 쓸 것
        int effectColumn = 0;
        for (int i = 0; i < eventEffectInfoTable.Count; i++)
        {
            if (eventEffectInfoTable[i]["ID"].Equals(textEffect))
            {
                effectColumn = i;
                break;
            }
        }

        string valueKey = string.Empty;
        int rewardKey = 0;
        int priorityRewardType = 0;

        List<string> tags = new();

        int tagCount = (int)eventEffectInfoTable[effectColumn]["PriorityTagCount"];
        for (int i = 0; i < tagCount; i++)
        {
            string tag = $"{eventEffectInfoTable[effectColumn][$"PriorityTag{i + 1}"]}";

            if (tag == string.Empty)
            {
                break;
            }
            Logger.Debug(tag);
            tags.Add(tag);
        }

        // 테이블에 태그와 연관된 이벤트가 없을 때는 연산하지 않고 바로 노멀 이펙트로 이동
        if (tags.Count == 0)
        {
            GetNormalEventEffect(ref valueKey, ref rewardKey, effectColumn);
        }
        else
        {
            GetPriorityTagEventEffect(ref valueKey, ref rewardKey, effectColumn, tags, ref priorityRewardType);
        }

        Logger.Debug($"{valueKey}");
        contentText.text = gm.GetStringByTable(valueKey);

        if (rewardKey == -1)
        {
            return;
        }

        switch (priorityRewardType)
        {
            case 0:
                object stringTableRewardKey = rewardKey;
                AddReward(stringTableRewardKey);
                break;
            case 1:
                ExecutionBuff(rewardKey);
                break;
            case 2:
                var itemInfo = gm.itemInfoList.Find(t => t["ID"].ToString().Equals(rewardKey.ToString()));
                stageReward.AddItem($"{rewardKey}", itemInfo["Item_Name"].ToString()
                    , itemInfo["Icon_Name"].ToString()
                    , itemInfo["Info"].ToString()
                    , "1", true);
                stageReward.gameObject.SetActive(true);
                stageReward.OnEventRewardPage();
                break;
        }
    }

    private void Init()
    {
        if (tree.CurNode == null)
            tree.CreateTreeGraph();

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

        gm = GameManager.Instance;
        eventInfoTable = gm.eventInfoList;
        supplyInfoTable = gm.supplyInfoList;
        currentSelectMissionTable = gm.currentSelectMission;
        eventEffectInfoTable = gm.eventEffectInfoList;
        enemyInfoTable = gm.enemyInfoList;
        enemySpawnTable = gm.enemySpawnList;

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
                var coll = attackableHero.GetComponent<CapsuleCollider>();
                coll.enabled = true;
                useHeroes.Add(attackableHero);

                supplyEventHeroImages[i].SetHeroInfo(attackableHero.GetUnitData());
            }
        }

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var text = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            choiceButtonTexts.Add(text);
        }

        clearUi.SetHeroes(useHeroes);
        readyCount = useHeroes.Count;

        FindObjectOfType<AutoButton>().ResetData(ref useHeroes);

        gm.SetDifferentColor();
        for (int i = 0; i < eventMaps.Count; i++)
        {
            BattleMapInfo battleMap = eventMaps[i].GetComponent<BattleMapInfo>();
            Light battleMapLigth = battleMap.GetLight();
            battleMapLigth.color = gm.GetMapLightColor();
            lights.Add(battleMapLigth);
        }

        DisabledAllMap(); 

        var fitPropertyFlags = gm.fitPropertyFlags;
        for (int i = 0; i < 3; i++)
        {
            if (fitPropertyFlags[i])
            {
                var buff = FindBuff((int)(currentSelectMissionTable[$"BonusID{i + 1}"]));
                foreach (var hero in useHeroes)
                {
                    hero.AddValueBuff(buff);
                }
            }
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
                NodeClearReward();
                MissionClear();
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

    public void SelectNextStage(int index)
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
            return;
        }
    }

    public void MissionFail()
    {
        Time.timeScale = 0;
        UIManager.Instance.ShowView(2);
    }
    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
    public void ResetHeroes()
    {
        EffectManager.Instance.DisabledAllEffect();

        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ResetData();
            useHeroes[i].RemoveAllBuff();
            useHeroes[i].SetMaxHp();
            useHeroes[i].SetEnabledPathFind(false);
            Utils.CopyPositionAndRotation(useHeroes[i].gameObject, gm.heroSpawnTransform);
        }

        for (int i = 0; i < unuseHeroes.Count; i++)
        {
            unuseHeroes[i].ResetData();
            unuseHeroes[i].RemoveAllBuff();
            unuseHeroes[i].SetMaxHp();
            unuseHeroes[i].SetEnabledPathFind(false);
            Utils.CopyPositionAndRotation(unuseHeroes[i].gameObject, gm.heroSpawnTransform);
        }
        gm.SetHeroesActive(false);
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

        currTriggerIndex++;
        for (int i = 0; i < useHeroes.Count; i++)
        {
            var pos = btMapTriggers[currTriggerIndex].heroSettingPositions[i];
            useHeroes[i].MoveNext(pos.transform.position);
            useHeroes[i].SetMoveSpeed(platformMoveSpeed);
        }

        float nextMaxZPos = btMapTriggers[currTriggerIndex].heroSettingPositions.Max(transform => transform.position.z);
        while (!btMapTriggers[currTriggerIndex].isTriggerEnter)
        {
            if (viewPoint.transform.position.z <= nextMaxZPos)
                viewPoint.transform.Translate(platformMoveSpeed * Time.deltaTime * platform.transform.forward);

            yield return null;
        }

        if (!btMapTriggers[currTriggerIndex].isMissionEnd)
        {
            if (btMapTriggers[currTriggerIndex].isLastTrigger)
            {
                ChoiceNextStageByNode();
            }
            else if (tree.CurNode.type == TreeNodeTypes.Threat)
            {
                SetHeroReturnPositioning(btMapTriggers[currTriggerIndex].heroSettingPositions);
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

        if (tree.CurNode.type == TreeNodeTypes.Villain)
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
        stageReward.gameObject.SetActive(true);
        
        if (tree.CurNode.type != TreeNodeTypes.Event && tree.CurNode.type != TreeNodeTypes.Supply)
            NodeClearReward();

        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.Idle);
        }

        tree.ShowTree(true);

        List<TreeNodeObject> childs = tree.CurNode.childrens;
        int count = childs.Count;
        for (int i = 0; i < count; i++)
        {
            int num = i;
            childs[i].nodeButton.onClick.AddListener(() => SelectNextStage(num));
        }
    }

    //private void ChoiceNextStage()
    //{
    //    TreeNodeObject thisNode = tree.CurNode;
    //    int count = thisNode.childrens.Count;
    //    for (int i = 0; i < count; i++)
    //    {
    //        choiceButtonTexts[i].text = $"{thisNode.childrens[i].type}";
    //        choiceButtons[i].gameObject.SetActive(true);
    //        //roadChoiceButtons[i].choiceIndex = i;
    //    }
    //}

    private void CreateRoad()
    {
        if (tree.CurNode.childrens.Count == 0)
        {
            return;
        }

        road = Instantiate(roadPrefab[tree.CurNode.childrens.Count - 1], platform.transform);
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

        btMapTriggers = currBtMgr.GetTriggers();
        platform = currBtMgr.GetPlatform();
        viewPoint = currBtMgr.GetViewPoint();
        viewPointInitPos = viewPoint.transform.position;
        cinemachine.Follow = viewPoint.transform;

        SpawnCurrMapAllEnemys();

        if (tree.CurNode.type == TreeNodeTypes.Threat)
        {
            enemyCountTxt.StartTimer();
            isMiddleBossAlive = true;
        }
        else
            enemyCountTxt.Count = currBtMgr.GetAllEnemyCount();

        btMapTriggers.Last().isLastTrigger = true;

        for (int i = 0; i < btMapTriggers.Count; i++)
        {
            btMapTriggers[i].isTriggerEnter = false;
        }

        // 임시 빌드용 코드
        //if (tree.CurNode.type == TreeNodeTypes.Villain)
        //{
        //    currBtMgr.battleMapType = BattleMapEnum.Normal;

        //    btMapTriggers[^2].enemys.Clear();
        //    btMapTriggers[^2].enemyColls.Clear();
        //    btMapTriggers[^2].enemySettingPositions[1].enemyPrefabs[0] = bossPrefab;
        //    for (int i = 0; i < btMapTriggers[^2].enemySettingPositions.Count; i++)
        //    {
        //        var enemy = btMapTriggers[^2].enemySettingPositions[i].SpawnEnemy();

        //        for (int j = 0; j < enemy.Count; j++)
        //        {
        //            btMapTriggers[^2].enemys.Add(enemy[j]);
        //            btMapTriggers[^2].enemys[j].SetPathFind();
        //            btMapTriggers[^2].AddEnemyColliders(enemy[j].GetComponent<CapsuleCollider>());
        //            btMapTriggers[^2].enemys[j].SetEnabledPathFind(false);
        //        }
        //    }

        //    btMapTriggers[^2].ResetEnemys();
        //    btMapTriggers[^2].ResetEnemyPositions();
        //}

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
        tree.ShowTree(false);
        stageReward.gameObject.SetActive(false);

        coFadeOut = StartCoroutine(CoFadeOut());

        if (tree.CurNode.type == TreeNodeTypes.Event)
        {
            var randomEvent = Random.Range((int)MapEventEnum.CivilianRescue, (int)MapEventEnum.Count);
            StartNextStage((MapEventEnum)randomEvent);
            return true;
        }
        else if (tree.CurNode.type == TreeNodeTypes.Supply)
        {
            StartNextStage(MapEventEnum.None);
            return true;
        }
        else
        {
            StartNextStage(MapEventEnum.None);
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
        switch (currBtMgr.GetBattleMapType())
        {
            case BattleMapEnum.Normal:
                enemyCountTxt.DieEnemy();
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
        if (enemy.GetUnitData().data.job == (int)CharacterJob.villain &&
            tree.CurNode.type == TreeNodeTypes.Threat)
        {
            DeadMiddleBoss();
            SetHeroReturnPositioning(btMapTriggers[currTriggerIndex].heroSettingPositions);
        }
        else
        {
            int count = btMapTriggers[triggerIndex].useEnemys.Count;
            if (count == 0)
            {
                SetHeroReturnPositioning(btMapTriggers[currTriggerIndex].heroSettingPositions);
            }
        }
    }

    public void EnemyTriggerEnter()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            if (!useHeroes[i].GetUnitState().Equals(UnitState.Battle))
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

    [ContextMenu("TEST")]
    public void NodeClearReward()
    {
        var influence = gm.currentSelectMission["Influence"];//세력
        int difficulty = (int)gm.currentSelectMission["Difficulty"]; //난이도
        var nodeType = tree.CurNode.type; //노드타입

        var missionInfoDifficulty = gm.missionInfoDifficulty[difficulty];
        var data = missionInfoDifficulty.Find(t => t["Influence"].Equals(influence));

        string colomId = string.Empty;
        string collomWeight = string.Empty;
        int itemCount = 0;
        switch (nodeType)
        {
            case TreeNodeTypes.None:
                break;
            case TreeNodeTypes.Root:
                colomId = "WinReward";
                collomWeight = "Weight";
                itemCount = 8;
                break;
            case TreeNodeTypes.Normal:
                colomId = "WinReward";
                collomWeight = "Weight";
                itemCount = 8;
                break;
            case TreeNodeTypes.Threat:
                colomId = "HardReward";
                collomWeight = "HWeight";
                itemCount = 5;
                break;
            case TreeNodeTypes.Supply:
                colomId = "ClearReward";
                collomWeight = "CWeight";
                itemCount = 3;
                break;
            //case TreeNodeTypes.Event:
            //    colomId = "ClearReward";
            //    collomWeight = "CWeight";
            //    itemCount = 3;
            //    return;
            case TreeNodeTypes.Villain:
                colomId = "HardReward";
                collomWeight = "HWeight";
                itemCount = 5;
                return;
            default:
                break;
        }

        int weight = 0;

        List<string> allItems = new();
        for (int i = 1; i < itemCount+1; i++)
        {
            string itemWeight = collomWeight + i.ToString();
            string itemKey = colomId + i.ToString();
            var value = (int)data[itemWeight];
            if (value == -1)
                continue;
            allItems.AddRange(Enumerable.Repeat(itemKey, value));
            weight += value;
        }

        var rewardsCode = data[allItems[Random.Range(0, weight)]];
        AddReward(rewardsCode);
    }

    private void AddReward(object key)
    {
        var rewardData = gm.compensationInfoList.Find(t => t["ID"].Equals(key));

        int maxItemCount = 10;
        string keyItem = "Item";
        string keyValue = "Value";
        for (int i = 1; i < maxItemCount + 1; i++)
        {
            if ((int)rewardData[$"{keyValue}{i}"] == -1)
                continue;

            var item = gm.itemInfoList.Find(t => t["ID"].Equals(rewardData[$"{keyItem}{i}"]));
            stageReward.AddItem(rewardData[$"{keyItem}{i}"].ToString(),
                item["Item_Name"].ToString(),
                item["Icon_Name"].ToString(),
                item["Info"].ToString(),
                rewardData[$"{keyValue}{i}"].ToString(),
                false);
        }
        var gold = gm.itemInfoList.Find(t => t["ID"].Equals(keyItem));
        if ((int)rewardData["Gold"] != -1)
            stageReward.AddGold(rewardData["Gold"].ToString());
    }

    private void DeadMiddleBoss()
    {
        KillAllEnemy(enemyTriggerIndex);
        enemyCountTxt.StopTimer();
        isMiddleBossAlive = false;
    }

    private void KillAllEnemy(int index)
    {
        for (int i = 0; i < btMapTriggers[index].enemySettingPositions.Count; i++)
        {
            btMapTriggers[index].enemySettingPositions[i].StopInfinityRespawn();
        }

        int useCount = btMapTriggers[index].useEnemys.Count;


        for (int i = useCount - 1; i >= 0; i--)
        {
            btMapTriggers[index].useEnemys[i].ChangeUnitState(UnitState.Die);
        }

        int unuseCount = btMapTriggers[index].enemys.Count;
        for (int i = 0; i < unuseCount; i++)
        {
            if (btMapTriggers[index].enemys[i].isAlive)
            {
                btMapTriggers[index].enemys[i].ChangeUnitState(UnitState.Die);
            }
        }
    }

    public void SpawnCurrMapAllEnemys()
    {
        // 보스 ID 찾기
        string bossID = $"{currentSelectMissionTable["BossID"]}";

        // 미션 테이블에서 노멀 몬스터들 담겨있는 키 랜덤 뽑기
        int nMonCount = (int)currentSelectMissionTable["NMonCount"];
        int randomEnemyCount = Random.Range(1, nMonCount + 1);
        string normalEnemysKey = $"{currentSelectMissionTable[$"NMon{randomEnemyCount}"]}";

        // 뽑은 키로 스폰 테이블에서 소환할 적들 찾기
        Dictionary<string, object> spawnTableNormalEnemys = new();
        for (int i = 0; i < enemySpawnTable.Count; i++)
        {
            if ($"{enemySpawnTable[i]["ID"]}".Equals(normalEnemysKey))
            {
                spawnTableNormalEnemys = enemySpawnTable[i];
                break;
            }
        }

        // 적들 id, 마리수, 레벨 담아두기
        List<string> monIds = new();
        List<int> monValues = new();
        List<int> monLevels = new();

        int monCount = (int)spawnTableNormalEnemys["MonCount"];
        for (int i = 1; i <= monCount; i++)
        {
            string monId = $"{spawnTableNormalEnemys[$"MonID{i}"]}";
            monIds.Add(monId);

            int monValue = (int)spawnTableNormalEnemys[$"Monval{i}"];
            monValues.Add(monValue);

            int monLevel = (int)spawnTableNormalEnemys[$"Mon{i}LV"];
            monLevels.Add(monLevel);
        }


        // 적들 데이터 담아두기
        List<Dictionary<string, object>> enemyData = new();
        for (int i = 0; i < enemyInfoTable.Count; i++)
        {
            for (int j = 0; j < monIds.Count; j++)
            {
                if ($"{enemyInfoTable[i]["ID"]}".Equals(monIds[j]))
                {
                    enemyData.Add(enemyInfoTable[j]);
                }
            }
        }

        Logger.Debug(enemyData.Count);

        for (int i = 0; i < btMapTriggers.Count; i++)
        {
            int posCount = btMapTriggers[i].enemySettingPositions.Count;
            for (int j = 0; j < posCount; j++)
            {
                int currPosEnemyCount = monValues[j];
                CharacterData data = new();
                data.name = $"{enemyData[j]["NAME"]}";
                data.job = (int)enemyData[j]["JOB"];
                data.moveSpeed = (int)enemyData[j]["MOVESPEED"];

                string atk = $"{enemyData[j]["ATK"]}";
                string def = $"{enemyData[j]["DEF"]}";
                string levelAtk = $"{enemyData[j]["Levelup_Atk"]}";
                string levelDef = $"{enemyData[j]["Levelup_Def"]}";
                string levelHp = $"{enemyData[j]["Levelup_HP"]}";
                string healthPoint = $"{enemyData[j]["HP"]}";
                string critical = $"{enemyData[j]["CRITICAL"]}";
                string criticalDmg = $"{enemyData[j]["CRITICALDAMAGE"]}";
                string evasion = $"{enemyData[j]["EVADE"]}";
                string accuracy = $"{enemyData[j]["ACCURACY"]}";

                data.baseDamage = float.Parse(atk);
                data.baseDamage = float.Parse(def);
                data.damageLevelCoefficient = float.Parse(levelAtk);
                data.defenseLevelCoefficient = float.Parse(levelDef);
                data.healthPointLevelCoefficient = float.Parse(levelHp);
                data.healthPoint = float.Parse(healthPoint);
                data.critical = float.Parse(critical);
                data.criticalDmg = float.Parse(criticalDmg);
                data.evasion = float.Parse(evasion);
                data.accuracy = float.Parse(accuracy);

                data.grade = 1;
                data.maxGrade = 5;

                int enemyPrefabIndex = 0;
                for (int k = 0; k < enemyPrefabs.Count; k++)
                {
                    if (enemyPrefabs[k].gameObject.name.Equals(data.name))
                    {
                        enemyPrefabIndex = k;
                        break;
                    }
                }

                for (int l = 0; l < currPosEnemyCount; l++)
                {
                    AttackableEnemy enemy = new();
                    enemy = Instantiate(enemyPrefabs[enemyPrefabIndex]);
                    enemy.SetUnitOriginData(data);
                    btMapTriggers[i].enemySettingPositions[j].SpawnAllEnemy(ref btMapTriggers[i].enemys, enemy);
                }
            }
        }
    }
}
