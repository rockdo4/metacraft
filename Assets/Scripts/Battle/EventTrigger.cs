using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EventTrigger : MonoBehaviour
{
    [Header("스테이지에 포함된 적들을 넣어주세요")]
    public List<GameObject> enemys;
    private List<NavMeshAgent> enemysNav = new();
    public List<GameObject> returnPoints = new();

    [SerializeField, Header("마지막 관문일 때 체크해주세요")]
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
