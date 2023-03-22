using System.Collections.Generic;
using UnityEngine;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> heroSettingPositions;
    [Header("적을 소환할 위치")]
    public List<EnemySpawningAndPositioning> enemySettingPositions;

    public List<AttackableUnit> enemys = new();        // 생성할 Enemy들
    public List<AttackableUnit> useEnemys = new();     // 현재 생성된 Enemy들

    [Header("스테이지 선택 트리거")]
    public bool isStageEnd = false;
    [Header("미션 종료 트리거")]
    public bool isMissionEnd = false;
    [Header("길목 트리거")]
    public bool isForkedRoad = false;
    [Header("마지막 트리거")] 
    public bool isLastTrigger = false;
    public bool isTriggerEnter = false;

    // 적이 포함된 트리거
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

    public void SetEnemysActive(bool set) // 임시
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
