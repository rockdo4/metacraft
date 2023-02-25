using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> heroSettingPositions;
    [Header("���� ��ȯ�� ��ġ�� �־��ּ���.")]
    public List<EnemySpawningAndPositioning> enemySettingPositions;

    public List<AttackableEnemy> enemys;
    public List<NavMeshAgent> enemysNav = new();

    public GameObject enemyPool; // Enemy GameObject���� �� �θ�

    [SerializeField, Header("������ ������ �� üũ���ּ���")]
    public bool isStageEnd = false;

    private void Start()
    {
        for (int i = 0; i < enemySettingPositions.Count; i++)
        {
            var enemy = enemySettingPositions[i].SpawnEnemy(enemyPool);
            enemys.Add(enemy);
        }

        for (int i = 0; i < enemys.Count; i++)
        {
            enemysNav.Add(enemys[i].GetComponent<NavMeshAgent>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<AttackableHero>().Equals(null))
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                enemys[i].SetBattle();
            }
        }
        else
        {
            var enemy = other.GetComponent<AttackableEnemy>();
            enemy.SetBattle();
        }
    }

    public void OnDead(AttackableEnemy enemy)
    {
        enemys.Remove(enemy);
    }
}
