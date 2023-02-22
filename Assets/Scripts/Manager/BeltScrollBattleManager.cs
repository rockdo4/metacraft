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
    
    public void Ready()
    {
        readyCount--;
        if (readyCount == 0 && !triggers[currTriggerIndex].isStageEnd)
        {
            readyCount = 3;
            StartCoroutine(MovingMap());
        }
    }

    private void TempMoveTest()
    {
        if (!triggers[currTriggerIndex].isStageEnd)
        {
            StartCoroutine(MovingMap());
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TempMoveTest();
        }
    }

    public List<Transform> GetNextTriggerHeroSettingPos()
    {
        if (triggers[currTriggerIndex + 1] != null)
            return triggers[currTriggerIndex + 1].settingPositions;

        return null;
    }

    // 클리어 시 호출할 함수 (적어둘 내용들 몰라서 아직 안 적음)
    public void StageClear()
    {

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
    }
}
