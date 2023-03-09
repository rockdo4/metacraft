using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    private List<TestBattleManager> battleManagerList = new();
    private List<CinemachineVirtualCamera> cinemachines = new();
    private List<Dictionary<string, object>> eventInfoTable;

    private MapEventEnum curEvent = MapEventEnum.Normal;
    private GameObject curMap;

    private GameManager gm;

    public void Awake()
    {
        for (int i = 0; i < eventMaps.Count; i++)
        {
            var btMgr = eventMaps[i].GetComponent<TestBattleManager>();
            var camera = eventMaps[i].GetComponent<CinemachineVirtualCamera>();
            battleManagerList.Add(btMgr);
            cinemachines.Add(camera);
        }

        gm = GameManager.Instance;
        eventInfoTable = gm.eventInfoList;
        // 이벤트 인포 테이블에서 가져오면 됨
        // 예시) eventInfoTable[0]["Explanation"]
        StartEvent(curEvent);
    }

    public void StartEvent(MapEventEnum ev) // 이벤트 시작
    {
        // 이벤트 시작하면 해당 맵 활성화 및 curMap에 활성화된 맵 할당해주고
        // SetActiveEventMap 사용해서 해당 맵 true시키기

        curEvent = ev;
        
        // 이벤트 ui 설정
        SetEventUiProperty(curEvent);

        // 각각 이벤트 발생하는거 넣어주기
        switch (ev)
        {
            case MapEventEnum.Normal:
                curMap = eventMaps[0];

                break;
            case MapEventEnum.Defense:
                curMap = eventMaps[1];
                break;
            case MapEventEnum.CivilianRescue:
                break;
            case MapEventEnum.NewbieHeroRescue:
                break;
            case MapEventEnum.Roadblock:
                break;
            case MapEventEnum.FloodRescue:
                break;
            case MapEventEnum.BombTrap:
                break;
            case MapEventEnum.BlockDoor:
                break;
            case MapEventEnum.VillainsSafe:
                break;
            case MapEventEnum.LostChild:
                break;
            case MapEventEnum.MagicJean:
                break;
            case MapEventEnum.OperationBoard:
                break;
        }

        SetActiveEventMap(true);
    }

    public void EndEvent() // 이벤트 관련 ui 꺼주기
    {
        
    }

    public void SetActiveEventMap(bool active) // 현재 이벤트 발생 맵 액티브 관리
    {
        curMap.SetActive(active);
    }

    public void SetEventUiProperty(MapEventEnum ev) // 이벤트의 내용 설정
    {
        if (ev == MapEventEnum.Normal || ev == MapEventEnum.Defense)
            return;

        switch (ev)
        {
            case MapEventEnum.CivilianRescue:
                break;
            case MapEventEnum.NewbieHeroRescue:
                break;
            case MapEventEnum.Roadblock:
                break;
            case MapEventEnum.FloodRescue:
                break;
            case MapEventEnum.BombTrap:
                break;
            case MapEventEnum.BlockDoor:
                break;
            case MapEventEnum.VillainsSafe:
                break;
            case MapEventEnum.LostChild:
                break;
            case MapEventEnum.MagicJean:
                break;
            case MapEventEnum.OperationBoard:
                break;
        }
    }
}
