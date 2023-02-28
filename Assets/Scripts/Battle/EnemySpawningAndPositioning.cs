using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawningAndPositioning : MonoBehaviour
{
    private Transform tr;
    [Header("������ �� �������� �־��ּ���")]
    public AttackableEnemy enemy;
    [SerializeField, Header("�ش� ��ġ�� ������ ���� �������� �����ּ���")]
    public int enemyCount;
    private int spawnCount;

    private void Awake()
    {
        tr = gameObject.transform;
    }

    public void SetRespawnPos(Transform tr) => this.tr = tr;
    public Vector3 GetRespawnPos() => tr.position;
    public void SetEnemy(AttackableEnemy enemy) => this.enemy = enemy;
    public AttackableEnemy GetEnemy() => enemy;

    private IEnumerator CoRespawn(GameObject parents, List<AttackableEnemy> enemyPool, float timer)
    {
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // ������
        enemyPool.Add(SpawnEnemy(parents));
    }
    private IEnumerator CoInfinityRespawn(GameObject parents, List<AttackableEnemy> enemyPool, float timer)
    {
        float saveTimer = timer;
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // ������ �� �ٽ� �ڷ�ƾ ����
        //var spawnEnemy = SpawnEnemy(parents);
        //spawnEnemy.ChangeUnitState(UnitState.Battle);
        //enemyPool.Add(spawnEnemy);

        enemyPool[spawnCount].gameObject.SetActive(true);
        enemyPool[spawnCount].ChangeUnitState(UnitState.Battle);

        spawnCount++;
        if (spawnCount == enemyCount)
            yield break;

        StartCoroutine(CoInfinityRespawn(parents, enemyPool, saveTimer));
    }

    public AttackableEnemy SpawnEnemy(GameObject parents)
    {
        return Instantiate(enemy, tr.position, enemy.gameObject.transform.rotation, parents.transform);
    }
    public void RespawnEnemy(GameObject parents, ref List<AttackableEnemy> enemyPool, float timer)
    {
        StartCoroutine(CoRespawn(parents, enemyPool, timer));
    }
    public void InfinityRespawn(GameObject parents, ref List<AttackableEnemy> enemyPool, float timer)
    {
        StartCoroutine(CoInfinityRespawn(parents, enemyPool, timer));
    }
    public void SpawnAllEnemy(GameObject parents, ref List<AttackableEnemy> enemyPool)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            var e = Instantiate(enemy, tr.position, enemy.gameObject.transform.rotation, parents.transform);
            enemyPool.Add(e);
        }
    }
}
