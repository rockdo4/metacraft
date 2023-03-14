using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemySpawningAndPositioning : MonoBehaviour
{
    private Transform tr;
    [Header("생성할 적 프리펩들을 넣어주세요")]
    public List<AttackableEnemy> enemyPrefabs;
    [SerializeField, Header("생성할 몬스터 마리수를 넣어주세요.")]
    public int enemyCount;
    [SerializeField, Header("해당 구역의 크기를 정해주세요.")]
    private int spawnRange;
    [Range(1, 20), Header("리스폰 시간을 설정해주세요.")]
    public float respawnTimer;

    private int spawnCount = 0;
    public List<AttackableEnemy> enemys = new();

    private void Awake()
    {
        tr = gameObject.transform;
    }

    public void SetRespawnPos(Transform tr) => this.tr = tr;
    public Vector3 GetRespawnPos() => tr.position;
    public void SetEnemy(AttackableEnemy enemy, int index) => enemyPrefabs[index] = enemy;
    public void SetAllEnemy(List<AttackableEnemy> enemys) => enemyPrefabs = enemys;
    public List<AttackableEnemy> GetEnemy() => enemyPrefabs;

    private IEnumerator CoRespawn(List<AttackableUnit> enemyPool, float timer)
    {
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // 리스폰
        var spawn = SpawnEnemy();

        for (int i = 0; i < spawn.Count; i++)
        {
            enemyPool.Add(spawn[i]);
            enemys.Add(spawn[i]);
        }
    }
    private IEnumerator CoInfinityRespawn(float timer)
    {
        float saveTimer = timer;
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // 리스폰 후 다시 코루틴 시작
        enemys[spawnCount].SetEnabledPathFind(true);
        enemys[spawnCount].gameObject.SetActive(true);
        enemys[spawnCount].ChangeUnitState(UnitState.Battle);

        spawnCount++;
        if (spawnCount == enemyCount)
        {
            spawnCount = 0;
            //yield break;
        }

        StartCoroutine(CoInfinityRespawn(saveTimer));
    }

    public List<AttackableEnemy> SpawnEnemy()
    {
        if (enemyPrefabs == null)
            return null;

        List<AttackableEnemy> enemys = new();

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            var enemy = Instantiate(enemyPrefabs[i], tr.position, enemyPrefabs[i].gameObject.transform.rotation, tr);
            enemys.Add(enemy);
        }

        return enemys;
    }
    public void RespawnEnemy(ref List<AttackableUnit> enemyPool, float timer)
    {
        StartCoroutine(CoRespawn(enemyPool, timer));
    }
    public void InfinityRespawn()
    {
        StartCoroutine(CoInfinityRespawn(respawnTimer));
    }
    public void SpawnAllEnemy(ref List<AttackableUnit> enemyPool)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            for (int j = 0; j < enemyPrefabs.Count; j++)
            {
                var e = Instantiate(enemyPrefabs[i], tr.position, enemyPrefabs[i].gameObject.transform.rotation, tr);
                enemyPool.Add(e);
                enemys.Add(e);
            }
        }
    }
}
