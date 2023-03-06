using System.Collections.Generic;
using UnityEngine;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> heroSettingPositions;
    [Header("���� ��ȯ�� ��ġ�� �־��ּ���.")]
    public List<EnemySpawningAndPositioning> enemySettingPositions;

    public List<AttackableEnemy> enemys = new();        // ������ Enemy��
    public List<AttackableEnemy> useEnemys = new();     // ���� ������ Enemy��

    [SerializeField, Header("�������� ���� ������ �� üũ���ּ���")]
    public bool isStageEnd = false;
    [SerializeField, Header("������ ���������� �� üũ���ּ���")]
    public bool isMissionEnd = false;

    public void OnDead(AttackableEnemy enemy)
    {
        enemys.Add(enemy);
        useEnemys.Remove(enemy);
    }

    public void InfinityRespawnEnemy()
    {
        for (int i = 0; i < enemySettingPositions.Count; i++)
        {
            float timer = Random.Range(1f, 2f);
            enemySettingPositions[i].InfinityRespawn(timer);
        }
    }

    public void AddUseEnemyList(AttackableEnemy enemy)
    {
        useEnemys.Add(enemy);
    }
    public void SubUseEnemyList(AttackableEnemy enemy)
    {
        useEnemys.Remove(enemy);
    }
    public void SpawnAllEnemy()
    {
        for (int i = 0; i < enemySettingPositions.Count; i++)
        {
            enemySettingPositions[i].SpawnAllEnemy(ref enemys);
        }

        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].gameObject.SetActive(false);
        }
    }
}
