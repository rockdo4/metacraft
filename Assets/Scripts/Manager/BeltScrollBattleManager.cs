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

    private Coroutine coMovingMap;
    private Coroutine coResetMap;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            thisNode.type = TreeNodeTypes.Boss;
            DestroyRoad();
            RemoveRoadTrigger();
            ResetRoads();
            triggers.Last().isMissionEnd = true;
        }
    }

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

        CreateRoad(platform);
        AddRoadTrigger();

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

    public override void GetEnemyList(ref List<AttackableUnit> enemyList)
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

    public override void SelectNextStage(int index)
    {
        base.SelectNextStage(index);
        base.OnReady();
        SetHeroReturnPositioning(roads[nodeIndex].fadeTrigger.heroSettingPositions);
        coMovingMap = StartCoroutine(CoMovingMap());
    }

    public override void OnReady()
    {
        readyCount--;

        if (readyCount == 0)
        {
            if (triggers[currTriggerIndex].isMissionEnd)
            {
                SetStageClear();
            }
            else if (triggers[currTriggerIndex + 1] != null && triggers[currTriggerIndex + 1].isStageEnd)
            {
                ChoiceNextStage();
            }
            else if (!triggers[currTriggerIndex].isStageEnd)
            {
                readyCount = useHeroes.Count;
                base.OnReady();
                coMovingMap = StartCoroutine(CoMovingMap());
            }
        }
    }

    IEnumerator CoMovingMap()
    {
        float curMaxZPos = 0f;
        float nextMaxZPos = 0f;
        float movePos = 0f;
        float platformMoveSpeedValue = 0f;

        yield return new WaitForSeconds(nextStageMoveTimer);

        curMaxZPos = platform.transform.position.z +
            triggers[currTriggerIndex].heroSettingPositions.Max(transform => transform.position.z);

        if (triggers[currTriggerIndex + 1].isStageEnd)
        {
            platformMoveSpeedValue = 1f;
            nextMaxZPos = roads[nodeIndex].fadeTrigger.heroSettingPositions.Max(transform => transform.position.z);
        }
        else
        {
            platformMoveSpeedValue = platformMoveSpeed;
            nextMaxZPos = triggers[currTriggerIndex + 1].heroSettingPositions.Max(transform => transform.position.z);
        }

        movePos = curMaxZPos - nextMaxZPos;
        currTriggerIndex++;

        while (platform.transform.position.z >= movePos)
        {
            platform.transform.Translate((Vector3.forward * platformMoveSpeedValue * Time.deltaTime) * -1);
            yield return null;
        }

        // 히어로들 배틀 상태로 전환
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.Battle);
        }
    }

    public override void MoveNextStage(float timer)
    {
        StopCoroutine(coMovingMap);
        base.MoveNextStage(timer);
        coResetMap = StartCoroutine(CoResetMap(timer));
    }
    private IEnumerator CoResetMap(float timer)
    {
        currTriggerIndex = 0;
        Logger.Debug("End!");

        yield return new WaitForSeconds(timer / Time.timeScale);

        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ResetData();
        }

        platform.transform.position = Vector3.zero;

        // 여기에 에너미들 바꿔주는 거랑 마리수 조정
        // 일단은 다시 소환하는걸로
        ResetStage();
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Invoke(nameof(OnReady), 1f);
        }
        enemyCountTxt.Count = GetAllEnemyCount();

        DestroyRoad();
        RemoveRoadTrigger();
        ResetRoads();
        CreateRoad(platform);
        AddRoadTrigger();

        if (thisNode.nodeNameText.text == "Boss")
        {
            triggers.Last().isMissionEnd = true;
        }

        // 페이드 아웃
        coFadeOut = StartCoroutine(CoFadeOut());
        yield break;
    }
}
