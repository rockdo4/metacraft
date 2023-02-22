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

    private void Start()
    {
        // Test
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Invoke("OnReady", 1f);
        }
    }

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
        if (useHeroes.Count == 0)
        {
            SetEnemyIdle();
        }
    }

    private void SetHeroReturnPositioning()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].SetReturnPos(triggers[currTriggerIndex].settingPositions[i]);
            useHeroes[i].SetReturn();
        }
    }
    private void SetEnemyIdle()
    {
        for (int i = 0; i < triggers[currTriggerIndex].enemys.Count; i++)
        {

        }

        SetStageFail();
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
        else if (readyCount == 0 && triggers[currTriggerIndex].isStageEnd)
        {
            SetStageClear();
        }
    }

    // 클리어 시 호출할 함수 (Ui 업데이트)
    private void SetStageClear()
    {
        ViewManager.Show(1);
        clearUi.Clear();
        Logger.Debug("Clear!");
    }
    private void SetStageFail()
    {
        PopUpManager.Instance.ShowPopupInHierarchy(2);
        Logger.Debug("Fail!");
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
            useHeroes[i].SetBattle();
        }
    }
}
