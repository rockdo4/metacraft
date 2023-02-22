using System.Collections;
using System.Collections.Generic;
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
        for (int i = 0; i < triggers.Count; i++)
        {
            if (readyCount == 0 && !triggers[i].isStageEnd)
            {
                readyCount = 3;
                StartCoroutine(MovingMap());
                return;
            }
        }
    }

    private void TempMoveTest()
    {
        for (int i = 0; i < triggers.Count; i++)
        {
            if (!triggers[i].isStageEnd)
            {
                StartCoroutine(MovingMap());
                return;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TempMoveTest();
        }
    }

    // 클리어 시 호출할 함수 (적어둘 내용들 몰라서 아직 안 적음)
    public void StageClear()
    {

    }

    IEnumerator MovingMap()
    {
        yield return new WaitForSeconds(nextStageMoveTimer);

        var curPos = platform.gameObject.transform.position.z + triggers[currTriggerIndex].settingPosition.position.z;
        var nextPos = triggers[currTriggerIndex + 1].settingPosition.position.z;
        var movePos = curPos - nextPos;

        currTriggerIndex++;

        while (platform.gameObject.transform.position.z >= movePos)
        {
            platform.gameObject.transform.Translate((Vector3.forward * platformMoveSpeed * Time.deltaTime) * -1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
