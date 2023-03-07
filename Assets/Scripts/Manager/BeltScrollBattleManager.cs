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
        CreateRoad(platform);
    }

    private int GetCurrEnemyCount()
    {
        return triggers[currTriggerIndex].useEnemys.Count;
    }

    public override void GetEnemyList(ref List<AttackableEnemy> enemyList)
    {
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
    }
    protected override void SetHeroReturnPositioning(List<Transform> pos)
    {
        base.SetHeroReturnPositioning(pos);
    }

    public override void OnReady()
    {
        readyCount--;

        if (readyCount == 0)
        {
            if (!triggers[currTriggerIndex].isStageEnd)
            {
                readyCount = useHeroes.Count;
                base.OnReady();
                StartCoroutine(MovingMap());
            }
            else if (triggers[currTriggerIndex].isStageEnd)
            {
                Logger.Debug("End!");
                for (int i = 0; i < useHeroes.Count; i++)
                {
                    useHeroes[i].ResetData();
                }
                platform.transform.position = Vector3.zero;

                // 여기에 에너미들 바꿔주는 거랑 마리수 조정
                // 일단은 다시 소환하는걸로
                ResetStage();
                DisableRoad();
                // 보여질 길목들 다시 enable 해주기


                // 페이드 아웃
                MoveNextStage();

                // test
                //SetStageClear();
            }
            else if (triggers[currTriggerIndex].isMissionEnd)
            {
                SetStageClear();
            }
        }
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
        if (!triggers[currTriggerIndex].isStageEnd)
        {
            for (int i = 0; i < useHeroes.Count; i++)
            {
                useHeroes[i].ChangeUnitState(UnitState.Battle);
            }
        }
    }
}
