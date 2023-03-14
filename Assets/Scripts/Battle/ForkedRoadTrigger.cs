using UnityEngine;

public class ForkedRoadTrigger : MapEventTrigger
{
    private BattleManager manager;
    private bool isEnter = false;

    private void Start()
    {
        manager = FindObjectOfType<BattleManager>();
        isForkedRoad = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEnter)
        {
            manager.MoveNextStage(2f);
            isEnter = true;
        }
    }
}
