using UnityEngine;

public class ForkedRoadTrigger : MapEventTrigger
{
    private BattleManager manager;
    public bool isEnter = false;

    private void Awake()
    {
        manager = FindObjectOfType<BattleManager>();
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
