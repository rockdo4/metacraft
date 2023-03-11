using UnityEngine;

public class ForkedRoadTrigger : MapEventTrigger
{
    private EventManager manager;
    public bool isEnter = false;

    private void Awake()
    {
        manager = FindObjectOfType<EventManager>();
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
