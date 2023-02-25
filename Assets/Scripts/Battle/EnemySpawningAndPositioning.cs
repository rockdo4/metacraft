using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningAndPositioning : MonoBehaviour
{
    private Transform tr;
    [Header("적 프리펩을 넣어주세요")]
    public AttackableEnemy enemy;
    private float respawnTimer = 1f;

    private void Awake()
    {
        tr = gameObject.transform;
    }

    public void SetRespawnPos(Transform tr) => this.tr = tr;
    public Vector3 GetRespawnPos() => tr.position;
    public void SetEnemy(AttackableEnemy enemy) => this.enemy = enemy;
    public AttackableEnemy GetEnemy() => enemy;
    public float GetRespawnTimer() => respawnTimer;

    private IEnumerator CoRespawn(GameObject parents, List<AttackableEnemy> enemyPool)
    {
        while (respawnTimer <= 0f)
        {
            respawnTimer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // 리스폰
        enemyPool.Add(SpawnEnemy(parents));
    }
    public void RespawnEnemy(GameObject parents, ref List<AttackableEnemy> enemyPool)
    {
        StartCoroutine(CoRespawn(parents, enemyPool));
    }
    public AttackableEnemy SpawnEnemy(GameObject parents)
    {
        return Instantiate(enemy, tr.position, enemy.gameObject.transform.rotation, parents.transform);
    }
}
