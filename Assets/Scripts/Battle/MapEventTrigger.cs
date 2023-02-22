using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MapEventTrigger : MonoBehaviour
{
    public Transform settingPosition;
    [Header("���������� ���Ե� ������ �־��ּ���")]
    public List<GameObject> enemys;
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
}
