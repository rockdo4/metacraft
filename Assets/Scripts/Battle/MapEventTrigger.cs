using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> settingPositions;
    [Header("스테이지에 포함된 적들을 넣어주세요")]
    public List<AttackableEnemy> enemys;
    public List<NavMeshAgent> enemysNav = new();

    [SerializeField, Header("마지막 관문일 때 체크해주세요")]
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

        // 네비게이션 처음부터 켜있을거라 Nav.enabled 필요없음
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
