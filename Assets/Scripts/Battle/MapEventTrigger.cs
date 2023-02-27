using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> heroSettingPositions;
    [Header("적을 소환할 위치를 넣어주세요.")]
    public List<EnemySpawningAndPositioning> enemySettingPositions;

    public List<AttackableEnemy> enemys;                // 현재 생성되어있는 enemy들
    public List<AttackableEnemy> useEnemys = new();     // 
    public List<NavMeshAgent> enemysNav = new();        // 생성되어있는 enemy들의 NavMeshAgent

    public GameObject enemyPool; // Enemy GameObject들이 들어갈 부모

    [SerializeField, Header("마지막 관문일 때 체크해주세요")]
    public bool isStageEnd = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AttackableHero>() != null)
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                enemysNav[i].enabled = true;
                enemys[i].ChangeUnitState(UnitState.Battle);
            }
        }
        else
        {
            AttackableEnemy enemy = other.GetComponent<AttackableEnemy>();
            AddUseEnemyList(enemy);
        }
    }

    public void OnDead(AttackableEnemy enemy)
    {
        enemys.Remove(enemy);
        useEnemys.Remove(enemy);
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
}
