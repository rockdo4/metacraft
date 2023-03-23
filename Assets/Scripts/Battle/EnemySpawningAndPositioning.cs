using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningAndPositioning : MonoBehaviour
{
    private Transform tr;
    [SerializeField, Range(1, 20), Header("�ش� ���� �ִ� ������ Ƚ�� (����� ���� 1)")]
    public int waveCount = 0;
    [SerializeField, Header("�ش� ������ ũ�⸦ �����ּ���.")]
    private int spawnRange;
    [Range(1, 20), Header("������ �ð��� �������ּ���.")]
    public float respawnTimer;
    public bool isMiddleBoss = false;

    private int spawnCount = 0;
    public List<List<AttackableEnemy>> enemys = new();
    private bool isInfinityRespawn = true;

    // �ӽ�
    public AttackableEnemy middleBoss;
    public int firstSpawnCount = 0;

    private void Awake()
    {
        tr = gameObject.transform;
        spawnRange = 5;

        if (waveCount == 0)
            waveCount = 1;
    }

    public void SetRespawnPos(Transform tr) => this.tr = tr;
    public Vector3 GetRespawnPos() => tr.position;

    private Coroutine coInfinityRespawn;
    private IEnumerator CoInfinityRespawn(float timer)
    {
        if (enemys.Count == 0)
            yield break;

        // ������ �� �ٽ� �ڷ�ƾ ����
        for (int i = 0; i < enemys[spawnCount].Count; i++)
        {
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

    public void ClearEnemysList()
    {
        enemys.Clear();
    }

    public void StopInfinityRespawn()
    {
        isInfinityRespawn = false;
    }

    public void InfinityRespawn()
    {
        spawnCount = 0;
        isInfinityRespawn = true;
        coInfinityRespawn = StartCoroutine(CoInfinityRespawn(respawnTimer));
    }

    public void SpawnRandomAreaEnemys (AttackableEnemy enemy)
    {
        Vector3 trPos = tr.position;

        Vector3 randomArea = UnityEngine.Random.insideUnitSphere * spawnRange;
        randomArea.y = 0f;
        randomArea.x += trPos.x;
        randomArea.z += trPos.z;

        enemy.gameObject.transform.position = randomArea;
        enemy.gameObject.transform.parent = tr;
    }

    public void SpawnAllEnemy
        (ref List<AttackableUnit> enemyPool, AttackableEnemy enemy, int wave)
    {
        //enemys.Add(new List<AttackableEnemy>());
        SpawnRandomAreaEnemys(enemy);
        enemyPool.Add(enemy);
        enemys[wave].Add(enemy);

        //firstSpawnCount++;
    }
}
