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
        EnableRoad();
        SetRoad();
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

    public override void SelectNextStage(int index)
    {
        base.SelectNextStage(index);
        base.OnReady();
        SetHeroReturnPositioning(enableRoads[nodeIndex].fadeTrigger.heroSettingPositions);
        Logger.Debug(enableRoads[nodeIndex].fadeTrigger.heroSettingPositions);
    }

    public override void OnReady()
    {
        readyCount--;

        if (readyCount == 0)
        {
            if (triggers[currTriggerIndex + 1].isStageEnd)
            {
                ChoiceNextStage();
            }
            else if (!triggers[currTriggerIndex].isStageEnd)
            {
                readyCount = useHeroes.Count;
                base.OnReady();
                coMovingMap = StartCoroutine(CoMovingMap());
            }
            else if (triggers[currTriggerIndex].isMissionEnd)
            {
                SetStageClear();
            }
        }
    }

    IEnumerator CoMovingMap()
    {
        if (triggers[currTriggerIndex + 1].isStageEnd)
        {
            yield break;
        }

        yield return new WaitForSeconds(nextStageMoveTimer);

        float curMaxZPos = platform.transform.position.z +
            triggers[currTriggerIndex].heroSettingPositions.Max(transform => transform.position.z);
        float nextMaxZPos = triggers[currTriggerIndex + 1].heroSettingPositions.Max(transform => transform.position.z);
        float movePos = curMaxZPos - nextMaxZPos;

        currTriggerIndex++;

        while (platform.transform.position.z >= movePos)
        {
            platform.transform.Translate((Vector3.forward * platformMoveSpeed * Time.deltaTime) * -1);
            yield return null;
        }

        // ����ε� ��Ʋ ���·� ��ȯ
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

        yield return new WaitForSeconds(timer);

        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ResetData();
        }

        DisableRoad();
        EnableRoad();
        SetRoad();

        platform.transform.position = Vector3.zero;

        // ���⿡ ���ʹ̵� �ٲ��ִ� �Ŷ� ������ ����
        // �ϴ��� �ٽ� ��ȯ�ϴ°ɷ�
        ResetStage();
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Invoke(nameof(OnReady), 1f);
        }
        enemyCountTxt.Count = GetAllEnemyCount();

        // ���̵� �ƿ�
        coFadeOut = StartCoroutine(CoFadeOut());
        yield break;
    }
}
