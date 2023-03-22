using System.Collections.Generic;
using UnityEngine;

public class MapEventTrigger : MonoBehaviour
{
    public List<Transform> heroSettingPositions;
    [Header("���� ��ȯ�� ��ġ")]
    public List<EnemySpawningAndPositioning> enemySettingPositions;

    public List<AttackableUnit> enemys = new();        // ������ Enemy��
    public List<AttackableUnit> useEnemys = new();     // ���� ������ Enemy��

    [Header("�������� ���� Ʈ����")]
    public bool isStageEnd = false;
    [Header("�̼� ���� Ʈ����")]
    public bool isMissionEnd = false;
    [Header("��� Ʈ����")]
    public bool isForkedRoad = false;
    [Header("������ Ʈ����")] 
    public bool isLastTrigger = false;
    public bool isTriggerEnter = false;

    public List<CapsuleCollider> enemyColls = new();

    public void OnDead(AttackableEnemy enemy)
    {
        //enemys.Add(enemy);
        useEnemys.Remove(enemy);
        //Destroy(enemy.gameObject);
    }

    public void InfinityRespawnEnemy()
    {
        for (int i = 0; i < enemySettingPositions.Count; i++)
        {
            enemySettingPositions[i].InfinityRespawn();
        }
    }

    public void AddUseEnemyList(AttackableUnit enemy)
    {
        useEnemys.Add(enemy);
    }
    public void SubUseEnemyList(AttackableUnit enemy)
    {
        useEnemys.Remove(enemy);
    }
    public void SpawnAllEnemy()
    {
        //for (int i = 0; i < enemySettingPositions.Count; i++)
        //{
        //    enemySettingPositions[i].SpawnAllEnemy(ref enemys);
        //}

        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].gameObject.SetActive(false);
        }

        AddEnemysCollider();
    }

    public void AddEnemysCollider()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            AddEnemyColliders(enemys[i].GetComponent<CapsuleCollider>());
        }
    }

    //public void ResetEnemys()
    //{
    //    for (int i = 0; i < enemys.Count; i++)
    //    {
    //        enemys[i].ResetData();
    //        enemys[i].RemoveAllBuff();
    //        enemys[i].gameObject.SetActive(true);

    //        if (enemyColls.Count > 0)
    //            enemyColls[i].enabled = true;

    //        enemys[i].SetEnabledPathFind(false);
    //        //enemys[i].gameObject.transform.position = enemySettingPositions[i].GetRespawnPos();
    //    }
    //}

    public void SetEnemysActive(bool set) // �ӽ�
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].gameObject.SetActive(set);
        }
    }

    //public void ResetEnemyPositions()
    //{
    //    for (int i = 0; i < enemys.Count; i++)
    //    {
    //        enemys[i].gameObject.transform.position = enemySettingPositions[i].GetRespawnPos();
    //    }
    //}

    public void AddEnemyColliders(CapsuleCollider coll)
    {
        enemyColls.Add(coll);
    }
}
