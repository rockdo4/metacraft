using UnityEngine;

public class ForkedRoadTrigger : MapEventTrigger
{
    private TestBattleManager manager;
    public bool isEnter = false;

    private void Awake()
    {
        manager = FindObjectOfType<TestBattleManager>();
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
