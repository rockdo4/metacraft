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
    [Header("스킵할 트리거")]
    public bool isSkip = false;
    [Header("길목 트리거")]
    public bool isForkedRoad = false;
    [Header("마지막 트리거")] 
    public bool isLastTrigger = false;

    public List<CapsuleCollider> enemyColls = new();

    private void Start()
    {
        if (enemySettingPositions.Count == 0)
            isSkip = true;
    }

    public void OnDead(AttackableEnemy enemy)
    {
        enemys.Add(enemy);
        useEnemys.Remove(enemy);
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

    public void ResetEnemys()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].ResetData();
            enemys[i].gameObject.SetActive(true);
            enemyColls[i].enabled = true;
            enemys[i].gameObject.transform.position = enemySettingPositions[i].GetRespawnPos();
            enemys[i].SetEnabledPathFind(false);
        }
    }

    public void AddEnemyColliders(CapsuleCollider coll)
    {
        enemyColls.Add(coll);
    }
}
