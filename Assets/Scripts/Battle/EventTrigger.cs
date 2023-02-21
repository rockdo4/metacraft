using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EventTrigger : MonoBehaviour
{
    [Header("���������� ���Ե� ������ �־��ּ���")]
    public List<GameObject> enemys;
    private List<NavMeshAgent> enemysNav = new();
    public List<GameObject> returnPoints = new();

    [SerializeField, Header("������ ������ �� üũ���ּ���")]
    private bool isStageEnd = false;

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
            enemysNav[i].SetDestination(other.transform.position);
        }
    }
}
