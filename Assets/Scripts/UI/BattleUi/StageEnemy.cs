using System.Collections;
using TMPro;
using UnityEngine;

public class StageEnemy : MonoBehaviour
{
    public TextMeshProUGUI countTxt;
    public int count;
    public float timer = 180f;
    private BattleManager btMgr;
    private Coroutine coStartTimmer;
    private bool startTimer;

    public int Count {
        get { return count; }
        set {
            if (value < 0)
                return;
            count = value;
            countTxt.text = $"���� �� : {count}";
        }
    }

    [ContextMenu("Test/DieEnemy")]
    public int DieEnemy() => --Count;
    [ContextMenu("Test/AddEnemy")]
    public int AddEnemy() => ++Count;

    public void StartTimer()
    {
        btMgr = FindAnyObjectByType<BattleManager>();
        coStartTimmer = StartCoroutine(CoStartTimer());
    }

    public void StopTimer()
    {
        if (startTimer)
        {
            StopCoroutine(coStartTimmer);
            startTimer = false;
            timer = 180f;
        }
    }

    private IEnumerator CoStartTimer()
    {
        startTimer = true;
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;
            countTxt.text = $"���� �ð� : {(int)timer}��";
            yield return null;
        }

        btMgr.MissionFail();
    }
}
