using System.Collections;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    public Transform tr;
    public AttackableEnemy respawnEnemy;
    public float respawnTimer = 1f;

    public void SetRespawnPos(Transform tr) => this.tr = tr;
    public Vector3 GetRespawnPos() => tr.position;
    public void SetRespawnEnemy(AttackableEnemy enemy) => respawnEnemy = enemy;
    public AttackableEnemy GetRespawnEnemy() => respawnEnemy;
    public float GetRespawnTimer() => respawnTimer;

    private IEnumerator CoRespawn()
    {
        while (respawnTimer <= 0f)
        {
            respawnTimer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // ¸®½ºÆù
    }

    public void RespawnEnemy()
    {
        StartCoroutine(CoRespawn());
    }
}
