using System.Collections.Generic;
using UnityEngine;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> heroSettingPositions;
    [Header("���� ��ȯ�� ��ġ")]
    public List<EnemySpawningAndPositioning> enemySettingPositions;

    public List<AttackableUnit> enemys = new();        // ������ Enemy��
    public List<AttackableUnit> useEnemys = new();     // ���� ������ Enemy��

    [Header("�������� ���� Ʈ����")]
    public bool isStageEnd = false;
    [Header("�̼� ���� Ʈ����")]
    public bool isMissionEnd = false;
    [Header("��� Ʈ����")]
    public bool isForkedRoad = false;
    [Header("������ Ʈ����")] 
    public bool isLastTrigger = false;
    public bool isTriggerEnter = false;

    // ���� ���Ե� Ʈ����
    public bool isEnemyTrigger;

    public void OnDead(AttackableEnemy enemy)
    {
        useEnemys.Remove(enemy);
        enemys.Remove(enemy);
    }

    public void InfinityRespawnEnemy()
    {
        for (int i = 0; i < enemySettingPositions.Count; i++)
        {
            enemySettingPositions[i].InfinityRespawn();
        }
    }

    public void AddUseEnemyList(AttackableUnit enemy)
    {
        useEnemys.Add(enemy);
    }
    public void SubUseEnemyList(AttackableUnit enemy)
    {
        useEnemys.Remove(enemy);
    }

    public void SetEnemysActive(bool set) // �ӽ�
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].gameObject.SetActive(set);
        }
    }

    public void ResetSpawnCount()
    {
        for (int i = 0; i < enemySettingPositions.Count; i++)
        {
            enemySettingPositions[i].firstSpawnCount = 0;
        }
    }
}
