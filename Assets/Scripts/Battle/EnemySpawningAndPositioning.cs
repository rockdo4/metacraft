using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningAndPositioning : MonoBehaviour
{
    private Transform tr;
    [Header("������ �� �������� �־��ּ���")]
    public AttackableEnemy enemy;
    [SerializeField, Header("������ ���� �������� �־��ּ���.")]
    public int enemyCount;
    private int spawnCount = 0;
    public List<AttackableEnemy> enemys = new();

    private void Awake()
    {
        tr = gameObject.transform;
    }

    public void SetRespawnPos(Transform tr) => this.tr = tr;
    public Vector3 GetRespawnPos() => tr.position;
    public void SetEnemy(AttackableEnemy enemy) => this.enemy = enemy;
    public AttackableEnemy GetEnemy() => enemy;

    private IEnumerator CoRespawn(List<AttackableUnit> enemyPool, float timer)
    {
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // ������
        var spawn = SpawnEnemy();
        enemyPool.Add(spawn);
        enemys.Add(spawn);
    }
    private IEnumerator CoInfinityRespawn(float timer)
    {
        float saveTimer = timer;
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // ������ �� �ٽ� �ڷ�ƾ ����
        enemys[spawnCount].SetEnabledPathFind(true);
        enemys[spawnCount].gameObject.SetActive(true);
        enemys[spawnCount].ChangeUnitState(UnitState.Battle);

        spawnCount++;
        if (spawnCount == enemyCount)
            yield break;

        StartCoroutine(CoInfinityRespawn(saveTimer));
    }

    public AttackableEnemy SpawnEnemy()
    {
        if (enemy == null)
            return null;

        return Instantiate(enemy, tr.position, enemy.gameObject.transform.rotation, tr);
    }
    public void RespawnEnemy(ref List<AttackableUnit> enemyPool, float timer)
    {
        StartCoroutine(CoRespawn(enemyPool, timer));
    }
    public void InfinityRespawn(float timer)
    {
        StartCoroutine(CoInfinityRespawn(timer));
    }
    public void SpawnAllEnemy(ref List<AttackableUnit> enemyPool)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            var e = Instantiate(enemy, tr.position, enemy.gameObject.transform.rotation, tr);
            enemyPool.Add(e);
            enemys.Add(e);
        }
    }
}
