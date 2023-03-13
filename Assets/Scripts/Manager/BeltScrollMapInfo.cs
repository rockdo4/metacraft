using UnityEngine;

public class BeltScrollMapInfo : BattleMapInfo
{
    private void Start()
    {
        FindEvManager();
        battleMapType = BattleMapEnum.BeltScroll;

        for (int i = 0; i < triggers.Count; i++)
        {
            for (int j = 0; j < triggers[i].enemySettingPositions.Count; j++)
            {
                var enemy = triggers[i].enemySettingPositions[j]?.SpawnEnemy();
                if (enemy == null)
                    break;

                triggers[i].enemys.Add(enemy);
                triggers[i].enemys[j].SetPathFind();
                triggers[i].AddEnemyColliders(enemy.GetComponent<CapsuleCollider>());
                triggers[i].enemys[j].SetEnabledPathFind(false);
            }
        }

        int count = GetAllEnemyCount();
        evManager.SetEnemyCountTxt(count);
    }
}
