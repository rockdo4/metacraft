using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

// 맵은 여러개 만들지 않을거임
// 벨트스크롤이면 기존 벨트스크롤대로 사용하고
// 방어전도 기존 방어전을 사용하되
// ui를 사용해서 이벤트를 관리할 예정

// 존재할 맵들
// 기존 벨트스크롤
// 기존 방어전
// 트리거 1칸있는 벨트스크롤 (보스 맵으로도 사용할 예정)
// 길막 오브젝트 있는 맵
// 등등... (아직 생각 안남)

public class EventManager : MonoBehaviour
{
    [Header("이벤트 발생 시 출력할 맵을 넣어주세요.")]
    public List<GameObject> eventMaps;
    private List<Dictionary<string, object>> eventInfoTable;

    private MapEventEnum curEvent = MapEventEnum.Normal;
    private GameObject curMap;

    public GameObject eventUi;
    [Header("선택지 버튼들")]
    public List<GameObject> choiceButtons;
    [Header("히어로 이미지가 들어갈 Ui")]
    public Image heroImage;
    [Header("이벤트 설명이 들어갈 텍스트 오브젝트")]
    public TextMeshProUGUI contentText;

    private List<TextMeshProUGUI> buttonTexts = new(); // 버튼들 내부의 텍스트들

    private List<string> heroNames = new();

    public void Awake()
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var text = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonTexts.Add(text);
        }

        GameManager gm = GameManager.Instance;

        eventInfoTable = gm.eventInfoList;
        // 이벤트 인포 테이블에서 가져오면 됨
        // 예시) eventInfoTable[0]["Explanation"]

        var useHeroes = gm.GetSelectedHeroes();
        for (int i = 0; i < useHeroes.Count; i++)
        {
            heroNames.Add(useHeroes[i].name);
        }

        StartEvent(curEvent);
    }
    
    public void StartEvent(MapEventEnum ev) // 이벤트 시작
    {
        // 이벤트 시작하면 해당 맵 활성화 및 curMap에 활성화된 맵 할당해주고
        // SetActiveEventMap 사용해서 해당 맵 true시키기
        curEvent = ev;

        // 이벤트 ui 및 맵 출력
        SetEventUiProperty(curEvent);
    }

    public void EndEvent() // 이벤트 관련 ui 꺼주기, 뭐 할거 없으면 함수 지우기
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        SetEventUiActive(false);
    }

    public void SetActiveEventMap(bool active) // 현재 이벤트 발생 맵 액티브 관리
    {
        curMap.SetActive(active);
    }

    public void SetEventUiProperty(MapEventEnum ev) // 이벤트의 내용 설정
    {
        if (curMap != null)
            SetActiveEventMap(false);

        if (ev == MapEventEnum.Normal)
        {
            curMap = eventMaps[0];
        }
        else if (ev == MapEventEnum.Defense)
        {
            curMap = eventMaps[1];
        }
        else
        {
            curMap = eventMaps[2];
            SetEventInfo(ev);
            SetEventUiActive(true);
        }

        SetActiveEventMap(true);
    }

    public void SetEventUiActive(bool active) => eventUi.SetActive(active);

    public void SetEventInfo(MapEventEnum ev)
    {
        int heroNameIndex = Random.Range(0, heroNames.Count);
        heroImage.sprite = GameManager.Instance.GetSpriteByAddress($"Icon_{heroNames[heroNameIndex]}");
        contentText.text = $"{eventInfoTable[(int)ev]["Explanation"]}";

        int textCount = (int)eventInfoTable[(int)ev][$"TextCount"];
        Logger.Debug(textCount);
        for (int i = 0; i < textCount; i++)
        {
            choiceButtons[i].SetActive(true);
            string text = $"{eventInfoTable[(int)ev][$"Select{i+1}Text"]}";
            buttonTexts[i].text = text;
        }
    }
}
