using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawningAndPositioning : MonoBehaviour
{
    private Transform tr;
    [Header("생성할 적 프리펩들을 넣어주세요")]
    public List<AttackableEnemy> enemyPrefabs;
    [SerializeField, Header("리스폰할 몬스터 웨이브 수를 넣어주세요.")]
    public int waveCount;
    [SerializeField, Header("해당 구역의 크기를 정해주세요.")]
    private int spawnRange;
    [Range(1, 20), Header("리스폰 시간을 설정해주세요.")]
    public float respawnTimer;
    [SerializeField, Header("중간보스가 포함되어 있으면 선택해주세요.")]
    private bool isMiddleBoss = false;

    private int spawnCount = 0;
    public List<List<AttackableEnemy>> enemys = new();

    // 임시
    public AttackableEnemy middleBoss;

    private void Awake()
    {
        tr = gameObject.transform;
    }

    public void SetRespawnPos(Transform tr) => this.tr = tr;
    public Vector3 GetRespawnPos() => tr.position;
    public void SetEnemy(AttackableEnemy enemy, int index) => enemyPrefabs[index] = enemy;
    public void SetAllEnemy(List<AttackableEnemy> enemys) => enemyPrefabs = enemys;
    public List<AttackableEnemy> GetEnemy() => enemyPrefabs;

    private Coroutine coInfinityRespawn;

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
            for (int j = 0; j < enemys.Count; j++)
            {
                enemys[j].Add(spawn[i]);
            }
        }
    }
    private IEnumerator CoInfinityRespawn(float timer)
    {
        yield return new WaitForSeconds(timer);

        // 리스폰 후 다시 코루틴 시작
        for (int i = 0; i < enemys[spawnCount].Count; i++)
        {
            enemys[spawnCount][i].SetEnabledPathFind(true);
            enemys[spawnCount][i].gameObject.SetActive(true);
            enemys[spawnCount][i].ChangeUnitState(UnitState.Battle);
        }

        if (isMiddleBoss)
            yield break;

        spawnCount++;
        if (spawnCount == waveCount)
        {
            spawnCount = 0;
            yield break;
        }

        coInfinityRespawn = StartCoroutine(CoInfinityRespawn(timer));
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
        coInfinityRespawn = StartCoroutine(CoInfinityRespawn(respawnTimer));
    }
    public void SpawnAllEnemy(ref List<AttackableUnit> enemyPool)
    {
        Vector3 trPos = tr.position;

        for (int i = 0; i < waveCount; i++) // 4
        {
            enemys.Add(new List<AttackableEnemy>());
            for (int j = 0; j < enemyPrefabs.Count; j++) // 3
            {
                Vector3 randomArea = UnityEngine.Random.insideUnitSphere * spawnRange;
                randomArea.y = 0f;
                randomArea.x += trPos.x;
                randomArea.z += trPos.z;

                var e = Instantiate(enemyPrefabs[j], randomArea, enemyPrefabs[j].gameObject.transform.rotation, tr);
                enemyPool.Add(e);
                enemys[i].Add(e);
            }

            if (isMiddleBoss)
                break;
        }

        if (isMiddleBoss)
        {
            int maxHp = enemys.SelectMany(x => x).Max(enemy => enemy.GetUnitData().data.healthPoint);
            for (int i = 0; i < enemys.Count; i++)
            {
                for (int j = 0; j < enemys[i].Count; j++)
                {
                    if (enemys[i][j].GetUnitData().data.healthPoint == maxHp)
                    {
                        middleBoss = enemys[i][j];
                        break;
                    }
                }
            }
        }
    }

    // 임시 함수들
    public AttackableEnemy GetMiddleBoss()
    {
        return middleBoss;
    }
    public bool GetMiddleBossIsAlive()
    {
        if (middleBoss == null)
            return true;

        if (middleBoss.GetUnitState() == UnitState.Die)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
