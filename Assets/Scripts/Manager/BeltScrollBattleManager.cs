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
            tree.CurNode.type = TreeNodeTypes.Boss;
            DestroyRoad();
            RemoveRoadTrigger();
            ResetRoads();
            triggers.Last().isMissionEnd = true;
        }
    }

    protected void Start()
    {
        StartFadeOut();

        for (int i = 0; i < triggers.Count; i++)
        {
            for (int j = 0; j < triggers[i].enemySettingPositions.Count; j++)
            {
                var enemy = triggers[i].enemySettingPositions[j]?.SpawnEnemy();
                if (enemy == null)
                    break;

                triggers[i].enemys.Add(enemy);
                triggers[i].AddEnemyColliders(enemy.GetComponent<CapsuleCollider>());
                triggers[i].enemys[j].SetEnabledPathFind(false);
            }
        }

        //CreateRoad(platform);
        //AddRoadTrigger();

        if (tree.CurNode.type == TreeNodeTypes.Normal || evManager.curEvent == MapEventEnum.Normal)
        {
            for (int i = 0; i < useHeroes.Count; i++)
            {
                Invoke(nameof(OnReady), 1f);
            }
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
            Logger.Debug("NextTrigger");
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
                ChoiceNextStageByNode();
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

        yield return new WaitForSeconds(nextStageMoveTimer);

        curMaxZPos = platform.transform.position.z +
            triggers[currTriggerIndex].heroSettingPositions.Max(transform => transform.position.z);

        if (!triggers[currTriggerIndex + 1].isStageEnd)
        {
            nextMaxZPos = triggers[currTriggerIndex + 1].heroSettingPositions.Max(transform => transform.position.z);
            movePos = curMaxZPos - nextMaxZPos;
            while (platform.transform.position.z >= movePos)
            {
                platform.transform.Translate((Vector3.forward * platformMoveSpeed * Time.deltaTime) * -1);
                yield return null;
            }

            currTriggerIndex++;

            // ????¥å? ??? ???¡¤? ???
            for (int i = 0; i < useHeroes.Count; i++)
            {
                useHeroes[i].ChangeUnitState(UnitState.Battle);
            }

            if (triggers[currTriggerIndex].useEnemys.Count == 0)
            {
                Logger.Debug("NextTrigger");
                ChoiceNextStageByNode();
            }
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

        yield return new WaitForSeconds(timer / Time.timeScale);

        if (OnNextStage())
        {
            Logger.Debug("OnNextStage");
            yield break;
        }

        // ???? ?????? ?????? ??? ?????? ????
        // ????? ??? ?????¡Æ??
        platform.transform.position = Vector3.zero;
        ResetStage();

        for (int i = 0; i < useHeroes.Count; i++)
        {
            Invoke(nameof(OnReady), 1f);
        }
        enemyCountTxt.Count = GetAllEnemyCount();

        CreateRoad(platform);
        AddRoadTrigger();

        if (tree.CurNode.type == TreeNodeTypes.Boss)
        {
            triggers.Last().isMissionEnd = true;
        }

        yield break;
    }

    public void TestOnReady()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Invoke(nameof(OnReady), 1f);
        }
    }
}
