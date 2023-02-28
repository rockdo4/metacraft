using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeltScrollBattleManager : TestBattleManager
{
    public GameObject platform;
    public float platformMoveSpeed;

    private int currTriggerIndex = 0;
    private float nextStageMoveTimer = 0f;

    private void Start()
    {
        for (int i = 0; i < triggers.Count; i++)
        {
            for (int j = 0; j < triggers[i].enemySettingPositions.Count; j++)
            {
                var enemy = triggers[i].enemySettingPositions[j].SpawnEnemy();
                triggers[i].enemys.Add(enemy);
                triggers[i].enemys[j].SetEnabledPathFind(false);
            }
        }

        // Test
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Invoke(nameof(OnReady), 1f);
        }

        enemyCountTxt.Count = GetAllEnemyCount();
    }
    private int GetCurrEnemyCount()
    {
        return triggers[currTriggerIndex].useEnemys.Count;
    }

    public override void GetEnemyList(ref List<AttackableEnemy> enemyList)
    {
        Logger.Debug($"Get! {triggers[currTriggerIndex].useEnemys.Count}");
        enemyList = triggers[currTriggerIndex].useEnemys;
    }

    public override void OnDeadEnemy(AttackableEnemy enemy)
    {
        base.OnDeadEnemy(enemy);
        triggers[currTriggerIndex].OnDead(enemy);
        if (triggers[currTriggerIndex].useEnemys.Count == 0)
        {
            SetHeroReturnPositioning(triggers[currTriggerIndex].heroSettingPositions);
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
    protected override void SetHeroReturnPositioning(List<Transform> pos)
    {
        base.SetHeroReturnPositioning(pos);
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
            yield return null;
        }

        // 히어로들 배틀 상태로 전환
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.Battle);
        }
    }
}
