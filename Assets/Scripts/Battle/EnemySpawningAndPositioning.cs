using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawningAndPositioning : MonoBehaviour
{
    private Transform tr;
    [Header("������ �� ��������� �־��ּ���")]
    public List<AttackableEnemy> enemyPrefabs;
    [SerializeField, Header("�������� ���� ���̺� ���� �־��ּ���.")]
    public int waveCount;
    [SerializeField, Header("�ش� ������ ũ�⸦ �����ּ���.")]
    private int spawnRange;
    [Range(1, 20), Header("������ �ð��� �������ּ���.")]
    public float respawnTimer;
    [SerializeField, Header("�߰������� ���ԵǾ� ������ �������ּ���.")]
    private bool isMiddleBoss = false;

    private int spawnCount = 0;
    public List<List<AttackableEnemy>> enemys = new();

    // �ӽ�
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

        // ������
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

        // ������ �� �ٽ� �ڷ�ƾ ����
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

    // �ӽ� �Լ���
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
