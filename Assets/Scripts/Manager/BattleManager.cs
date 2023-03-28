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

    private MapEventEnum curEvent = MapEventEnum.None;
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
    private List<AttackableUnit> useHeroes = new();
    private List<AttackableUnit> unuseHeroes = new();
    public StageEnemy enemyCountTxt;
    public ClearUiController clearUi;
    public SendOfficePopUp sendOfficePopUp;
    public Image fadePanel;
    public TreeMapSystem tree;
    private int nodeIndex;
    public List<RoadChoiceButton> roadChoiceButtons;
    private List<TextMeshProUGUI> choiceButtonTexts = new();
    private Coroutine coFadeIn;
    private Coroutine coFadeOut;
    private int readyCount;
    public List<GameObject> roadPrefab;
    private List<MapEventTrigger> roads = new();
    private GameObject road;

    // BeltScrollManager
    private GameObject platform;
    private float platformMoveSpeed = 7f;
    private int currTriggerIndex = 0;
    private float nextStageMoveTimer = 0f;
    private Coroutine coMovingMap;
    private Coroutine coResetMap;

    private BattleMapInfo currBtMgr;
    private List<MapEventTrigger> btMapTriggers = new();

    private GameObject viewPoint;
    private Vector3 viewPointInitPos;

    public CinemachineVirtualCamera cinemachine;
    private int enemyTriggerIndex = 0;                          // 방어전에 쓰일것 (에너미 스폰하는 트리거)
    public bool isMiddleBossAlive;
    private GameManager gm;

    public Button eventExitButton;

    //이벤트, 보급 등에서 사용하는 버프리스트
    public List<BuffInfo> buffList;
    private List<int> supplyEffectKey = new();

    [Header("생성할 적 프리펩들을 넣어주세요")]
    public List<AttackableEnemy> enemyPrefabs = new();
    [Header("생성할 빌런들을 넣어주세요")]
    public List<AttackableEnemy> villainPrefabs = new();
    private AttackableEnemy villain;
    private AttackableEnemy middleBoss;

    public Button autoButton;
    public Button speedButton;

    private void Start()
    {
        // 밸런스 테스트용 임시 코드
        GameManager gm = GameManager.Instance;
        gm.enemySpawnList = CSVReader.Read("EnemySpawnTest");
        gm.enemyInfoList = CSVReader.Read("EnemyInfoTest");
        Init();
        StartNextStage(curEvent);

        sendOfficePopUp.checkButton.onClick.AddListener(() => { SetHeroesReady(); });
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
    public void EndSupply(int idx)
    {
        SetActiveUi(supplyUi, supplyButtons, false, supplyButtons.Count);
        for (int i = 0; i < heroUiList.Count; i++)
        {
            heroUiList[i].gameObject.SetActive(true);
        }

        switch (idx)
        {
            case 0:
                ExecutionBuff(80260002);
                break;
            case 1:
                List<object> supplyList = new();
                string supplyId = $"{currentSelectMissionTable["SupplyID"]}";
                var data = GameManager.Instance.supplyInfoList.Find(t => t["ID"].ToString().CompareTo(supplyId) == 0);

                for (int i = 0; i < 20; i++)
                {
                    var key = data[$"Effect{i + 1}"];
                    if ((int)key != -1)
                        supplyList.Add(key);
                    else
                        continue;
                }

                var choiceSupply = supplyList[Random.Range(0, supplyList.Count)];
                ExecutionBuff((int)choiceSupply);
                break;
            case 2:
                stageReward.gameObject.SetActive(true);
                clearUi.rewards.SaveItems();
                sendOfficePopUp.gameObject.SetActive(true);
                sendOfficePopUp.SetItems(stageReward.rewards);
                clearUi.ResetUi();
                stageReward.ResetData();
                break;
            default:
                break;
        }
    }

    private void StartNextStage(MapEventEnum ev)
    {
        curEvent = ev;

        SetStageEvent(ev);
        StartStage();

        switch (tree.CurNode.type)
        {
            case TreeNodeTypes.Root:
            case TreeNodeTypes.Normal:
            case TreeNodeTypes.Villain:
                for (int i = 0; i < useHeroes.Count; i++)
                    Invoke(nameof(OnReady), 3f);
                break;
            default:
                break;
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
                curMap = eventMaps[3];
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

            string supplyTextId = $"{supplyInfoTable[index]["Supply_text"]}";
            string findStringTable = gm.GetStringByTable(supplyTextId);
            supplyContentText.text = findStringTable;

            // 찾은 인덱스로 해당 줄의 데이터들을 불러옴
            for (int i = 0; i < supplyButtons.Count; i++)
            {
                supplyButtons[i].SetActive(true);

                string textId = $"{supplyInfoTable[index][$"Choice{i + 1}_text"]}";

                string stringTableChoiceText = gm.GetStringByTable(textId);
                supplyButtonTexts[i].text = stringTableChoiceText;

                int effectCount = (int)supplyInfoTable[index]["EffectCount"];
                int randomEffectKey = Random.Range(1, effectCount);
                int effectKey = (int)supplyInfoTable[index][$"Effect{randomEffectKey}"];
                supplyEffectKey.Add(effectKey);
            }
        }
        else
        {
            int heroNameIndex = Random.Range(0, useHeroes.Count);
            string heroName = useHeroes[heroNameIndex].GetUnitData().data.name;
            battleEventHeroImage.sprite = gm.GetSpriteByAddress($"icon_{heroName}");
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
        stageReward.nowRewards.Clear();
        Logger.Debug("stageReward.nowRewards.Clear");
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
                    , itemInfo["Sort"].ToString()
                    , itemInfo["DataID"].ToString()
                    , "1", true); ;
                stageReward.gameObject.SetActive(true);
                stageReward.OnEventRewardPage();
                break;
        }
    }

    private void Init()
    {
        gm = GameManager.Instance;
        if (tree.CurNode == null)
        {
            if (gm.playerData.isTutorial)
            {
                tree.CreateTreeGraph();
                autoButton.interactable = false;
                speedButton.interactable = false;
            }
            else
            {
                int difficulty = (int)gm.currentSelectMission["Difficulty"];
                tree.CreateTreeGraph(difficulty);
            }
        }

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
            //lights.Add(battleMapLigth);
        }

        DisabledAllMap();

        // 보스 ID 찾기
        string villainID = $"{currentSelectMissionTable["VillainID"]}";        
        //villain
        for (int i = 0; i < villainPrefabs.Count; i++)
        {
            if (villainPrefabs[i].name.Equals(villainID))
            {
                PlayBGM(i);
                break;
            }
            PlayBGM(0);
        }

        for (int i = 0; i < 3; i++)
        {
            if (gm.fitPropertyFlags[i])
            {
                var buff = FindBuff((int)(currentSelectMissionTable[$"BonusID{i + 1}"]));
                foreach (var hero in useHeroes)
                {
                    hero.AddValueBuff(buff);
                }
            }
        }
    }

    private void PlayBGM(int index)
    {
        int bgmIndex = 0;
        switch(index)
        {
            case 0:
                bgmIndex = 6;
                break;
            case 1:
                bgmIndex = 4;
                break;
            case 2:
                bgmIndex = 5;
                break;
        }
        AudioManager.Instance.PlayBGM(bgmIndex);
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
        if(tree.CurNode.type.Equals(TreeNodeTypes.Villain))
        {
            Invoke(nameof(PlayBossBGM), 3f);
        }
        readyCount = useHeroes.Count;
        int childCount = prevNode.childrens.Count;

        for (int i = 0; i < childCount; i++)
        {
            prevNode.childrens[i].nodeButton.onClick.RemoveAllListeners();
        }

        SetHeroReturnPositioning(roads[nodeIndex].heroSettingPositions);
    }
    private void PlayBossBGM()
    {
        int index = 0;
        switch (AudioManager.Instance.GetCurrBGMIndex())
        {
            case 6:
                index = 7;
                break;
            case 4:
                index = 8;
                break;
            case 5:
                index = 13;
                break;
        }
        AudioManager.Instance.ChageBGMOnlyFadeOut(index);
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
        // 튜토리얼 테스트하기 위해서 주석처리
        //if (gm.playerData.isTutorial)
        //    gm.playerData.isTutorial = false;

        stageReward.gameObject.SetActive(true);
        UIManager.Instance.ShowView(1);
        clearUi.SetData(btMapTriggers[currTriggerIndex].isMissionEnd);
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

        ResetThisHeroes(useHeroes);
        ResetThisHeroes(unuseHeroes);
        gm.SetHeroesActive(false);
    }

    private void ResetThisHeroes(List<AttackableUnit> heroes)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            heroes[i].ResetData();
            heroes[i].RemoveAllBuff();
            heroes[i].SetMaxHp();
            heroes[i].SetEnabledPathFind(false);
            Utils.CopyPositionAndRotation(heroes[i].gameObject, gm.heroSpawnTransform);
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
                // 삭제해도 될듯
            }
            else if (btMapTriggers[currTriggerIndex].enemys.Count == 0 &&
                btMapTriggers[currTriggerIndex].useEnemys.Count == 0)
            {
                SetHeroReturnPositioning(btMapTriggers[currTriggerIndex].heroSettingPositions);
            }
            else if (!btMapTriggers[currTriggerIndex].isLastTrigger)
            {
                for (int i = 0; i < useHeroes.Count; i++)
                {
                    useHeroes[i].ChangeUnitState(UnitState.Battle);
                }
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
        if (tree.CurNode.type != TreeNodeTypes.Supply)
            stageReward.gameObject.SetActive(true);
        else
        {
            for (int i = 0; i < useHeroes.Count; i++)
            {
                useHeroes[i].ChangeUnitState(UnitState.Idle);
            }
            OnClickClearUiButton();
            return;
        }

        if (tree.CurNode.type != TreeNodeTypes.Event && tree.CurNode.type != TreeNodeTypes.Supply)
            NodeClearReward();

        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.Idle);
        }

        UIManager.Instance.ShowView(1);
        clearUi.SetData(btMapTriggers[currTriggerIndex].isMissionEnd);
    }
    public void OnClickClearUiButton()
    {
        clearUi.ResetUi();
        UIManager.Instance.ShowView(0);
        tree.ShowTree(true);

        List<TreeNodeObject> childs = tree.CurNode.childrens;
        int count = childs.Count;
        for (int i = 0; i < count; i++)
        {
            int num = i;
            childs[i].nodeButton.onClick.AddListener(() => SelectNextStage(num));
            childs[i].nodeButton.onClick.AddListener(() => AudioManager.Instance.PlayUIAudio(0));
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
        road.transform.rotation = currBtMgr.roadTr.transform.rotation;
        //roads = road.GetComponentsInChildren<ForkedRoad>().ToList();
        roads = road.GetComponentsInChildren<MapEventTrigger>().ToList();
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
        
        if (tree.CurNode.type == TreeNodeTypes.Villain)
        {
            btMapTriggers.Last().isMissionEnd = true;
        }

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

        for (int i = 0; i < btMapTriggers.Count; i++)
        {
            btMapTriggers[i].enemys.Clear();

            for (int j = 0; j < btMapTriggers[i].enemySettingPositions.Count; j++)
            {
                btMapTriggers[i].enemySettingPositions[j].ClearEnemysList();
                btMapTriggers[i].enemySettingPositions[j].middleBoss = null;
                btMapTriggers[i].enemySettingPositions[j].isMiddleBoss = false;
                btMapTriggers[i].enemySettingPositions[j].firstSpawnCount = 0;
            }
        }
    }
    private void RemoveRoadTrigger()
    {
        for (int i = 0; i < btMapTriggers.Count; i++)
        {
            if (!btMapTriggers[i].isForkedRoad)
                continue;

            btMapTriggers.Remove(btMapTriggers[i]);
        }
    }
    private bool OnNextStage()
    {
        tree.ShowTree(false);
        stageReward.gameObject.SetActive(false);

        coFadeOut = StartCoroutine(CoFadeOut());

        if (tree.CurNode.type == TreeNodeTypes.Event)
        {
            if (gm.playerData.isTutorial)
            {
                // 길막! 이벤트
                StartNextStage(MapEventEnum.Roadblock);
            }
            else
            {
                var randomEvent = Random.Range((int)MapEventEnum.CivilianRescue, (int)MapEventEnum.Count);
                StartNextStage((MapEventEnum)randomEvent);
            }
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
            btMapTriggers.Add(roads[i]);
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
                EnemyCountCheck(enemy, enemyTriggerIndex);
                break;
        }
    }

    private void EnemyCountCheck(AttackableEnemy enemy, int triggerIndex)
    {
        btMapTriggers[triggerIndex].OnDead(enemy);
        if (enemy.GetUnitData().data.job == (int)CharacterJob.elite &&
            tree.CurNode.type == TreeNodeTypes.Threat)
        {
            middleBoss = null;
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

    public void NodeClearReward()
    {
        stageReward.nowRewards.Clear();
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
                clearUi.baseExp = 0;
                return;
            //case TreeNodeTypes.Event:
            //    colomId = "ClearReward";
            //    collomWeight = "CWeight";
            //    itemCount = 3;
            //    return;
            case TreeNodeTypes.Villain:
                colomId = "ClearReward";
                collomWeight = "CWeight";
                itemCount = 3;
                break;
            default:
                break;
        }

        int weight = 0;

        List<string> allItems = new();
        for (int i = 1; i < itemCount + 1; i++)
        {
            string itemWeight = $"{collomWeight}{i}";
            string itemKey = $"{colomId}{i}";
            var value = (int)data[itemWeight];
            if (value == -1)
                continue;
            allItems.AddRange(Enumerable.Repeat(itemKey, value));
            weight += value;
        }

        var rewardsCode = data[allItems[Random.Range(0, weight)]];
        AddReward(rewardsCode);
        if(btMapTriggers[currTriggerIndex].isMissionEnd)
        {
            UIManager.Instance.ShowView(1);
            clearUi.nodeButton.gameObject.SetActive(false);
            clearUi.lastNodeButton.gameObject.SetActive(true);
        }
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
                item["Sort"].ToString(),
                item["DataID"].ToString(),
                rewardData[$"{keyValue}{i}"].ToString(),
                false);
        }
        var gold = gm.itemInfoList.Find(t => t["ID"].Equals(keyItem));
        if ((int)rewardData["Gold"] != -1)
            stageReward.AddGold(rewardData["Gold"].ToString());

        var baseExp = (int)rewardData["HeroExp"];
        clearUi.baseExp = baseExp == -1 ? 0 : baseExp;
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

        int unuseCount = btMapTriggers[index].enemys.Count - 1;
        for (int i = unuseCount; i >= 0; i--)
        {
            if (btMapTriggers[index].enemys[i].isAlive)
            {
                btMapTriggers[index].enemys[i].ChangeUnitState(UnitState.Die);
            }
            else
            {
                Destroy(btMapTriggers[index].enemys[i].gameObject);
            }
        }
    }

    public void SpawnCurrMapAllEnemys()
    {
        int bossTriggerIndex = 0;
        if (tree.CurNode.type == TreeNodeTypes.Villain)
        {
            for (int btMapTriggerCount = btMapTriggers.Count - 1; btMapTriggerCount >= 0; btMapTriggerCount--)
            {
                if (btMapTriggers[btMapTriggerCount].enemySettingPositions.Count > 0)
                {
                    bossTriggerIndex = btMapTriggerCount;
                    break;
                }
            }
        }

        for (int i = 0; i < btMapTriggers.Count; i++)
        {
            // 뽑은 키로 스폰 테이블에서 소환할 적들 찾기
            Dictionary<string, object> spawnTableNormalEnemys = new();
            Dictionary<string, object> spawnTableHardEnemys = new();

            if (tree.CurNode.type == TreeNodeTypes.Threat)
            {
                int hMonCount = (int)currentSelectMissionTable["HMonCount"];
                int randomHEnemyCount = Random.Range(1, hMonCount + 1);
                string threatEnemysKey = $"{currentSelectMissionTable[$"HMon{randomHEnemyCount}"]}";
                SetEnemySpawnTable(ref spawnTableHardEnemys, threatEnemysKey);
            }
            else
            {
                // 미션 테이블에서 노멀 몬스터들 담겨있는 키 랜덤 뽑기
                int nMonCount = (int)currentSelectMissionTable["NMonCount"];
                int randomEnemyCount = Random.Range(1, nMonCount + 1);
                string normalEnemysKey = $"{currentSelectMissionTable[$"NMon{randomEnemyCount}"]}";
                SetEnemySpawnTable(ref spawnTableNormalEnemys, normalEnemysKey);

                if (tree.CurNode.type == TreeNodeTypes.Villain && i == bossTriggerIndex)
                {
                    string villainEnemysKey = $"{currentSelectMissionTable["Villain"]}";
                    SetEnemySpawnTable(ref spawnTableNormalEnemys, villainEnemysKey);

                    //Logger.Debug($"Normal Key : {normalEnemysKey} / Boss Key : {villainEnemysKey}");
                }
            }

            // 적들 id, 마리수, 레벨 담아두기
            List<string> monIds = new();
            List<int> monValues = new();
            List<int> monLevels = new();

            if (tree.CurNode.type == TreeNodeTypes.Threat)
            {
                AddEnemySpawnTable(spawnTableHardEnemys, monIds, monValues, monLevels);
            }
            else
            {
                AddEnemySpawnTable(spawnTableNormalEnemys, monIds, monValues, monLevels);
            }

            // 적들 ID 찾아서 InfoTable 한 줄씩 담아두기
            List<Dictionary<string, object>> enemyData = new();
            for (int j = 0; j < monIds.Count; j++)
            {
                for (int k = 0; k < enemyInfoTable.Count; k++)
                {
                    string id = $"{enemyInfoTable[k]["ID"]}";
                    if (id.Equals(monIds[j]))
                    {
                        enemyData.Add(enemyInfoTable[k]);
                    }
                }
            }

            // 설치된 포지션의 카운트만큼 순회
            int posCount = btMapTriggers[i].enemySettingPositions.Count;
            for (int j = 0; j < posCount; j++)
            {
                // 프리펩 내부 순회
                for (int l = 0; l < enemyPrefabs.Count; l++)
                {
                    // 데이터 내부 순회
                    for (int k = 0; k < enemyData.Count; k++)
                    {
                        // 내부에서 이름 찾기
                        string enemyName = $"{enemyData[k]["NAME"]}";
                        int job = (int)enemyData[k]["JOB"];
                        // 찾음
                        if (enemyPrefabs[l].gameObject.name.Equals(enemyName))
                        {
                            // 생성해야하는 wave(리스폰할 횟수)당 해당 위치에 테이블의 마릿수만큼 소환
                            int currPosEnemyCount = monValues[k];
                            int waveCount = btMapTriggers[i].enemySettingPositions[j].waveCount;

                            //Logger.Debug($"Name : {enemyName} / MonCount : {currPosEnemyCount} / Trigger : {i}");

                            for (int wave = 0; wave < waveCount; wave++)
                            {
                                if (tree.CurNode.type == TreeNodeTypes.Threat)
                                {
                                    if (job == (int)CharacterJob.elite && middleBoss != null)
                                        break;
                                }
                                else if (tree.CurNode.type == TreeNodeTypes.Villain)
                                {
                                    if (job == (int)CharacterJob.villain && villain != null)
                                        break;
                                }
                                else
                                {
                                    if (job == (int)CharacterJob.villain)
                                        break;
                                }    

                                btMapTriggers[i].enemySettingPositions[j].enemys.Add(new List<AttackableEnemy>());
                                for (int s = 0; s < currPosEnemyCount; s++)
                                {
                                    var enemy = Instantiate(enemyPrefabs[l]);
                                    enemy.gameObject.SetActive(false);
                                    SetEnemyLiveData(enemyData, enemy);

                                    int saveWave = wave;
                                    int saveI = i;
                                    int saveJ = j;
                                    if (tree.CurNode.type == TreeNodeTypes.Threat && job == (int)CharacterJob.elite)
                                    {
                                        middleBoss = enemy;
                                        saveWave = 0;
                                        enemy.SetEnabledPathFind(true);
                                        enemy.ChangeUnitState(UnitState.Battle);
                                        btMapTriggers[i].enemySettingPositions[j].isMiddleBoss = true;
                                        btMapTriggers[i].enemySettingPositions[j].middleBoss = enemy;

                                        currPosEnemyCount = 0;
                                    }
                                    else if (tree.CurNode.type == TreeNodeTypes.Villain && job == (int)CharacterJob.villain)
                                    {
                                        villain = enemy;
                                        saveI = bossTriggerIndex;
                                        saveJ = 0;
                                        saveWave = 0;
                                        currPosEnemyCount = 0;
                                        btMapTriggers[saveI].enemySettingPositions[saveJ].enemys.Add(new List<AttackableEnemy>());
                                    }

                                    btMapTriggers[saveI].enemySettingPositions[saveJ].SpawnAllEnemy(ref btMapTriggers[saveI].enemys, enemy, saveWave);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void SetEnemyLiveData(List<Dictionary<string, object>> enemyData, AttackableEnemy enemy)
    {
        // 밸런스 테스트용 코드
        string key = enemy.GetUnitData().originData.name;
        int eiCount = enemyData.Count;
        for (int idx = 0; idx < eiCount; idx++)
        {
            string compareKey = enemyData[idx]["NAME"].ToString().ToLower();
            if (!key.Equals(compareKey))
            {
                //Logger.Debug($"{key} / {compareKey}");
                continue;
            }

            LiveData ld = enemy.GetUnitData().data;

            ld.level = (int)gm.currentSelectMission["Level"];

            ld.baseDamage = float.Parse(enemyData[idx]["ATK"].ToString());     // 일반 공격 데미지
            ld.baseDefense = float.Parse(enemyData[idx]["DEF"].ToString());     // 방어력
            ld.healthPoint = float.Parse(enemyData[idx]["HP"].ToString());   // 최대 체력
            ld.moveSpeed = float.Parse(enemyData[idx]["MOVESPEED"].ToString());       // 이동 속도. 범위, 초기값 설정 필요
            ld.critical = float.Parse(enemyData[idx]["CRITICAL"].ToString());     // 크리티컬 확률
            ld.criticalDmg = float.Parse(enemyData[idx]["CRITICALDAMAGE"].ToString());  // 크리티컬 데미지 배율
            ld.accuracy = float.Parse(enemyData[idx]["ACCURACY"].ToString());     // 명중률
            ld.evasion = float.Parse(enemyData[idx]["EVADE"].ToString());      // 회피율

            enemy.LevelupStats(ld.level - 1,
                float.Parse(enemyData[idx]["Levelup_Atk"].ToString()),
                float.Parse(enemyData[idx]["Levelup_Def"].ToString()),
                float.Parse(enemyData[idx]["Levelup_HP"].ToString()));
            //Logger.Debug($"{key} 적용 완료");
        }
    }

    private void AddEnemySpawnTable(Dictionary<string, object> table, List<string> ids, List<int> values, List<int> levels)
    {
        int monCount = (int)table["MonCount"];
        for (int j = 1; j <= monCount; j++)
        {
            string monId = $"{table[$"MonID{j}"]}";
            ids.Add(monId);

            int monValue = (int)table[$"Monval{j}"];
            values.Add(monValue);

            int monLevel = (int)table[$"Mon{j}LV"];
            levels.Add(monLevel);
        }
    }

    private void SetEnemySpawnTable(ref Dictionary<string, object> spawnTable, string nameKey)
    {
        for (int j = 0; j < enemySpawnTable.Count; j++)
        {
            if ($"{enemySpawnTable[j]["ID"]}".Equals(nameKey))
            {
                spawnTable = enemySpawnTable[j];
                break;
            }
        }
    }
}