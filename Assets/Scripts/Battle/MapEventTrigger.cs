using System.Collections.Generic;
using UnityEngine;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> heroSettingPositions;
    [Header("적을 소환할 위치를 넣어주세요.")]
    public List<EnemySpawningAndPositioning> enemySettingPositions;

    public List<AttackableEnemy> enemys = new();        // 생성할 Enemy들
    public List<AttackableEnemy> useEnemys = new();     // 현재 생성된 Enemy들

    [SerializeField, Header("마지막 관문일 때 체크해주세요")]
    public bool isStageEnd = false;

    public void OnDead(AttackableEnemy enemy)
    {
        useEnemys.Remove(enemy);
        enemy.gameObject.SetActive(false);
        enemys.Add(enemy);
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
