using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> heroSettingPositions;
    [Header("���� ��ȯ�� ��ġ�� �־��ּ���.")]
    public List<EnemySpawningAndPositioning> enemySettingPositions;

    public List<AttackableEnemy> enemys = new();        // ������ Enemy��
    public List<AttackableEnemy> useEnemys = new();     // ���� ������ Enemy��

    public GameObject enemyPool; // Enemy GameObject���� �� �θ�

    [SerializeField, Header("������ ������ �� üũ���ּ���")]
    public bool isStageEnd = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AttackableHero>() != null)
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                enemys[i].SetEnabledPathFind(true);
                enemys[i].ChangeUnitState(UnitState.Battle);
                AddUseEnemyList(enemys[i]);
                enemys.Remove(enemys[i]);
            }
        }
        else
        {
            AttackableEnemy enemy = other.GetComponent<AttackableEnemy>();
            AddUseEnemyList(enemy);
            enemys.Remove(enemy);
        }
    }

    public void OnDead(AttackableEnemy enemy)
    {
        useEnemys.Remove(enemy);
        enemy.gameObject.SetActive(false);
        enemys.Add(enemy);
    }

    // Test
    public void TestRespawnAllEnemy()
    {
        for (int i = 0; i < enemySettingPositions.Count; i++)
        {
            enemySettingPositions[i].RespawnEnemy(enemyPool, ref enemys, 1f);
        }
    }

    public void TestInfinityRespawnEnemy()
    {
        for (int i = 0; i < enemySettingPositions.Count; i++)
        {
            float timer = Random.Range(1f, 2f);
            enemySettingPositions[i].InfinityRespawn(enemyPool, ref enemys, timer);
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
            enemySettingPositions[i].SpawnAllEnemy(enemyPool, ref enemys);
        }

        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].gameObject.SetActive(false);
        }
    }
}
