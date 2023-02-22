using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeltScrollBattleManager : TestBattleManager
{
    public GameObject platform;
    public float platformMoveSpeed;

    public List<MapEventTrigger> triggers;
    private int currTriggerIndex = 0;
    private int readyCount = 3;
    private float nextStageMoveTimer = 0f;

    public void GetEnemyList(ref List<AttackableEnemy> enemyList)
    {
        enemyList = triggers[currTriggerIndex].enemys;
    }
    public void GetHeroList(ref List<AttackableHero> heroList)
    {
        heroList = useHeroes;
    }

    public void OnDeadEnemy(AttackableEnemy enemy)
    {
        triggers[currTriggerIndex].OnDead(enemy);
        if (triggers[currTriggerIndex].enemys.Count == 0)
        {
            SetHeroReturnPositioning();
        }
    }
    public void OnDeadHero(AttackableHero hero)
    {
        useHeroes.Remove(hero);
        readyCount = useHeroes.Count;
    }

    private void SetHeroReturnPositioning()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].SetReturnPos(triggers[currTriggerIndex].settingPositions[i]);
            useHeroes[i].SetReturn();
        }
    }

    public void OnReady()
    {
        readyCount--;
        if (readyCount == 0 && !triggers[currTriggerIndex].isStageEnd)
        {
            readyCount = useHeroes.Count;
            for (int i = 0; i < useHeroes.Count; i++)
            {
                useHeroes[i].SetMoveNext();
            }
            StartCoroutine(MovingMap());
        }
        else if (triggers[currTriggerIndex].isStageEnd)
        {
            SetStageClear();
        }
    }

    private void TempMoveTest()
    {
        if (!triggers[currTriggerIndex].isStageEnd)
        {
            for (int i = 0; i < useHeroes.Count; i++)
            {
                useHeroes[i].SetMoveNext();
            }
            StartCoroutine(MovingMap());
        }
        else
        {
            SetStageClear();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TempMoveTest();
        }
    }

    // 클리어 시 호출할 함수 (Ui 업데이트)
    private void SetStageClear()
    {
        Logger.Debug("Clear!");
    }

    IEnumerator MovingMap()
    {
        yield return new WaitForSeconds(nextStageMoveTimer);

        var curMaxZPos = platform.transform.position.z + 
            triggers[currTriggerIndex].settingPositions.Max(transform => transform.position.z);
        var nextMaxZPos = triggers[currTriggerIndex + 1].settingPositions.Max(transform => transform.position.z);
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
            useHeroes[i].SetTestBattle();
        }
    }
}
