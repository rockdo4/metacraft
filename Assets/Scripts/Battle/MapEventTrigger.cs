using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> settingPositions;
    [Header("���������� ���Ե� ������ �־��ּ���")]
    public List<AttackableEnemy> enemys;
    private List<NavMeshAgent> enemysNav = new();

    [SerializeField, Header("������ ������ �� üũ���ּ���")]
    public bool isStageEnd = false;

    private void Start()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemysNav.Add(enemys[i].GetComponent<NavMeshAgent>());
            enemysNav[i].enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemysNav[i].enabled = true;
            enemysNav[i].SetDestination(other.transform.position);
        }
    }

    public void OnDead(AttackableEnemy enemy)
    {
        enemys.Remove(enemy);
    }
}
