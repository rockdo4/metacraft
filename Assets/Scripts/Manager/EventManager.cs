using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// ���� ������ ������ ��������
// ��Ʈ��ũ���̸� ���� ��Ʈ��ũ�Ѵ�� ����ϰ�
// ������� ���� ������� ����ϵ�
// ui�� ����ؼ� �̺�Ʈ�� ������ ����

// ������ �ʵ�
// ���� ��Ʈ��ũ��
// ���� �����
// Ʈ���� 1ĭ�ִ� ��Ʈ��ũ�� (���� �����ε� ����� ����)
// �渷 ������Ʈ �ִ� ��
// ���... (���� ���� �ȳ�)

public class EventManager : MonoBehaviour
{
    [Header("�̺�Ʈ �߻� �� ����� ���� �־��ּ���.")]
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
        // �̺�Ʈ ���� ���̺��� �������� ��
        // ����) eventInfoTable[0]["Explanation"]
        StartEvent(curEvent);
    }

    public void StartEvent(MapEventEnum ev) // �̺�Ʈ ����
    {
        // �̺�Ʈ �����ϸ� �ش� �� Ȱ��ȭ �� curMap�� Ȱ��ȭ�� �� �Ҵ����ְ�
        // SetActiveEventMap ����ؼ� �ش� �� true��Ű��

        curEvent = ev;
        
        // �̺�Ʈ ui ����
        SetEventUiProperty(curEvent);

        // ���� �̺�Ʈ �߻��ϴ°� �־��ֱ�
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

    public void EndEvent() // �̺�Ʈ ���� ui ���ֱ�
    {
        
    }

    public void SetActiveEventMap(bool active) // ���� �̺�Ʈ �߻� �� ��Ƽ�� ����
    {
        curMap.SetActive(active);
    }

    public void SetEventUiProperty(MapEventEnum ev) // �̺�Ʈ�� ���� ����
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
