using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> settingPositions;

    // ���������� �̸� �־�δ°� �ƴ϶� ���Ե� ��(������)��� �ؼ� �� ��ġ�� Instantiate ���ֱ�
    [Header("���������� ���Ե� ������ �־��ּ���")]
    public List<AttackableEnemy> enemys;
    public List<NavMeshAgent> enemysNav = new();

    [SerializeField, Header("������ ������ �� üũ���ּ���")]
    public bool isStageEnd = false;

    private void Start()
    {
        // Instantiate ���ֱ�
        // instantiate�� ������Ʈ���� nav ��������

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
