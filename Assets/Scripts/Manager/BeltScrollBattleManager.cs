using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BeltScrollBattleManager : TestBattleManager
{
    public GameObject platform;
    public float platformMoveSpeed;

    private int currTriggerIndex = 0;
    private int readyCount = 3;
    private float nextStageMoveTimer = 0f;

    private void Start()
    {
        for (int i = 0; i < triggers.Count; i++)
        {
            for (int j = 0; j < triggers[i].enemySettingPositions.Count; j++)
            {
                var enemy = triggers[i].enemySettingPositions[j].SpawnEnemy(triggers[i].enemyPool);
                triggers[i].enemys.Add(enemy);
                triggers[i].enemysNav.Add(triggers[i].enemys[j].GetComponent<NavMeshAgent>());
                triggers[i].enemysNav[j].enabled = false;
            }
        }

        // Test
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Invoke(nameof(OnReady), 1f);
        }

        enemyCountTxt.Count = GetAllEnemyCount();
    }

    public override void GetEnemyList(ref List<AttackableEnemy> enemyList)
    {
        enemyList = triggers[currTriggerIndex].enemys;
    }

    public override void OnDeadEnemy(AttackableEnemy enemy)
    {
        base.OnDeadEnemy(enemy);
        triggers[currTriggerIndex].OnDead(enemy);
        if (triggers[currTriggerIndex].enemys.Count == 0)
        {
            SetHeroReturnPositioning();
        }
    }
    public override void OnDeadHero(AttackableHero hero)
    {
        base.OnDeadHero(hero);
        readyCount = useHeroes.Count;
        if (useHeroes.Count == 0)
        {
            SetEnemyIdle();
        }
    }

    private void SetHeroReturnPositioning()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].SetReturnPos(triggers[currTriggerIndex].heroSettingPositions[i]);
            useHeroes[i].ChangeUnitState(UnitState.ReturnPosition);
        }
    }
    private void SetEnemyIdle()
    {
        for (int i = 0; i < triggers[currTriggerIndex].enemys.Count; i++)
        {

        }

        SetStageFail();
    }

    public override void OnReady()
    {
        readyCount--;
        if (readyCount == 0 && !triggers[currTriggerIndex].isStageEnd)
        {
            readyCount = useHeroes.Count;
            base.OnReady();
            StartCoroutine(MovingMap());
        }
        else if (readyCount == 0 && triggers[currTriggerIndex].isStageEnd)
        {
            SetStageClear();
        }
    }

    // 클리어 시 호출할 함수 (Ui 업데이트)
    private void SetStageClear()
    {
        UIManager.Instance.ShowView(1);
        clearUi.Clear();
        Logger.Debug("Clear!");
    }
    private void SetStageFail()
    {
        Time.timeScale = 0;
        UIManager.Instance.ShowView(2);
        Logger.Debug("Fail!");
    }

    IEnumerator MovingMap()
    {
        yield return new WaitForSeconds(nextStageMoveTimer);

        var curMaxZPos = platform.transform.position.z + 
            triggers[currTriggerIndex].heroSettingPositions.Max(transform => transform.position.z);
        var nextMaxZPos = triggers[currTriggerIndex + 1].heroSettingPositions.Max(transform => transform.position.z);
        var movePos = curMaxZPos - nextMaxZPos;

        currTriggerIndex++;

        while (platform.transform.position.z >= movePos)
        {
            platform.transform.Translate((Vector3.forward * platformMoveSpeed * Time.deltaTime) * -1);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // 히어로들 배틀 상태로 전환
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.Battle);
        }
    }
}
