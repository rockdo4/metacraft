using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> settingPositions;
    [Header("���������� ���Ե� ������ �־��ּ���")]
    public List<AttackableEnemy> enemys;
    public List<NavMeshAgent> enemysNav = new();

    [SerializeField, Header("������ ������ �� üũ���ּ���")]
    public bool isStageEnd = false;

    private void Start()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemysNav.Add(enemys[i].GetComponent<NavMeshAgent>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].SetBattle();
        }

        // �׺���̼� ó������ �������Ŷ� Nav.enabled �ʿ����
        //var enemy = other.GetComponent<AttackableEnemy>();
        ////var nav = enemy.GetComponent<NavMeshAgent>();

        ////nav.enabled = true;
        //enemy.SetBattle();
    }

    public void OnDead(AttackableEnemy enemy)
    {
        enemys.Remove(enemy);
    }
}
