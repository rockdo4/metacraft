using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemySpawningAndPositioning : MonoBehaviour
{
    private Transform tr;
    //[Header("생성할 적 프리펩들을 넣어주세요")]
    //public List<AttackableEnemy> enemyPrefabs = new();
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
    private bool isInfinityRespawn = true;

    // 임시
    public AttackableEnemy middleBoss;

    private void Awake()
    {
        tr = gameObject.transform;
    }

    public void SetRespawnPos(Transform tr) => this.tr = tr;
    public Vector3 GetRespawnPos() => tr.position;
    //public void SetEnemy(AttackableEnemy enemy, int index) => enemyPrefabs[index] = enemy;
    //public void SetAllEnemy(List<AttackableEnemy> enemys) => enemyPrefabs = enemys;
    //public List<AttackableEnemy> GetEnemy() => enemyPrefabs;

    private Coroutine coInfinityRespawn;
    private IEnumerator CoInfinityRespawn(float timer)
    {
        // 리스폰 후 다시 코루틴 시작
        for (int i = 0; i < enemys[spawnCount].Count; i++)
        {
            enemys[spawnCount][i].SetEnabledPathFind(true);
            enemys[spawnCount][i].gameObject.SetActive(true);
            enemys[spawnCount][i].ChangeUnitState(UnitState.Battle);
            enemys[spawnCount][i].isAlive = true;
        }

        if (isMiddleBoss)
            yield break;

        spawnCount++;
        if (spawnCount == waveCount)
        {
            spawnCount = 0;
            yield break;
        }

        yield return new WaitForSeconds(timer);

        if (isInfinityRespawn)
            coInfinityRespawn = StartCoroutine(CoInfinityRespawn(timer));
    }

    public void StopInfinityRespawn()
    {
        isInfinityRespawn = false;
    }

    //public void SpawnEnemy(AttackableEnemy enemyPrefab)
    //{
    //    var enemy = Instantiate(enemyPrefab, tr.position, enemyPrefab.gameObject.transform.rotation, tr);
    //    enemys.Add(enemy);

    //}

    //public List<AttackableEnemy> SpawnEnemy()
    //{
    //    if (enemyPrefabs == null)
    //        return null;

    //    List<AttackableEnemy> enemys = new();
    //    for (int i = 0; i < enemyPrefabs.Count; i++)
    //    {
    //        var enemy = Instantiate(enemyPrefabs[i], tr.position, enemyPrefabs[i].gameObject.transform.rotation, tr);
    //        enemys.Add(enemy);
    //    }

    //    return enemys;
    //}

    private void ResetPositionEnemys()
    {
        Vector3 trPos = tr.position;

        for (int i = 0; i < enemys.Count; i++) // 4
        {
            for (int j = 0; j < enemys[i].Count; j++)
            {
                Vector3 randomArea = UnityEngine.Random.insideUnitSphere * spawnRange;
                randomArea.y = 0f;
                randomArea.x += trPos.x;
                randomArea.z += trPos.z;

                enemys[i][j].gameObject.transform.position = randomArea;
            }
        }
    }

    public void InfinityRespawn()
    {
        ResetPositionEnemys();

        spawnCount = 0;
        isInfinityRespawn = true;
        coInfinityRespawn = StartCoroutine(CoInfinityRespawn(respawnTimer));
    }

    public void SpawnRandomAreaEnemys
        (int index, int enemySpawnCount, ref List<AttackableUnit> enemyPool, AttackableEnemy enemyPrefab, CharacterData data)
    {
        Vector3 trPos = tr.position;

        enemys.Add(new List<AttackableEnemy>());
        for (int j = 0; j < enemySpawnCount; j++)
        {
            Vector3 randomArea = UnityEngine.Random.insideUnitSphere * spawnRange;
            randomArea.y = 0f;
            randomArea.x += trPos.x;
            randomArea.z += trPos.z;

            var e = Instantiate(enemyPrefab, randomArea, enemyPrefab.gameObject.transform.rotation, tr);
            e.SetUnitOriginData(data);

            enemyPool.Add(e);
            enemys[index].Add(e);
        }
    }

    public void SpawnAllEnemy
        (int enemySpawnCount, ref List<AttackableUnit> enemyPool, AttackableEnemy enemyPrefab, CharacterData data)
    {
        for (int i = 0; i < waveCount; i++)
        {
            SpawnRandomAreaEnemys(i, enemySpawnCount, ref enemyPool, enemyPrefab, data);
            if (isMiddleBoss)
                break;
        }

        if (isMiddleBoss)
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                for (int j = 0; j < enemys[i].Count; j++)
                {
                    if (enemys[i][j].GetUnitData().data.job == (int)CharacterJob.villain)
                    {
                        middleBoss = enemys[i][j];
                        break;
                    }
                }
            }
        }
    }
}
