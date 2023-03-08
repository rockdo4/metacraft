using System.Collections.Generic;
using UnityEngine;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> heroSettingPositions;
    [Header("적을 소환할 위치를 넣어주세요.")]
    public List<EnemySpawningAndPositioning> enemySettingPositions;

    public List<AttackableUnit> enemys = new();        // 생성할 Enemy들
    public List<AttackableUnit> useEnemys = new();     // 현재 생성된 Enemy들

    [SerializeField, Header("스테이지 선택 구간일 때 체크해주세요")]
    public bool isStageEnd = false;
    [SerializeField, Header("마지막 스테이지일 때 체크해주세요")]
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
            enemys[i].gameObject.transform.position = enemySettingPositions[i].GetRespawnPos();
            enemys[i].SetEnabledPathFind(false);
        }
    }
}
