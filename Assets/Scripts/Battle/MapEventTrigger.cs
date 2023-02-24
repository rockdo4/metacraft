using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> settingPositions;

    // 스테이지에 미리 넣어두는게 아니라 포함될 적(프리펩)들로 해서 각 위치에 Instantiate 해주기
    [Header("스테이지에 포함된 적들을 넣어주세요")]
    public List<AttackableEnemy> enemys;
    public List<NavMeshAgent> enemysNav = new();

    [SerializeField, Header("마지막 관문일 때 체크해주세요")]
    public bool isStageEnd = false;

    private void Start()
    {
        // Instantiate 해주기
        // instantiate한 오브젝트들의 nav 가져오기

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
