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

    private MapEventEnum curEvent = MapEventEnum.Normal;
    private GameObject curMap;

    public void Awake()
    {
        for (int i = 0; i < eventMaps.Count; i++)
        {
            var btMgr = eventMaps[i].GetComponent<TestBattleManager>();
            var camera = eventMaps[i].GetComponent<CinemachineVirtualCamera>();
            battleManagerList.Add(btMgr);
            cinemachines.Add(camera);
        }

        StartEvent(curEvent);
    }

    public void StartEvent(MapEventEnum ev) // �̺�Ʈ ����
    {
        // �̺�Ʈ �����ϸ� �ش� �� Ȱ��ȭ �� curMap�� Ȱ��ȭ�� �� �Ҵ����ְ�
        // SetActiveEventMap ����ؼ� �ش� �� true��Ű��

        curEvent = ev;

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
}
