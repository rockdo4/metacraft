using System.Collections;
using UnityEngine;

public class Rewards : MonoBehaviour
{
    [SerializeField]
    private int count;

    public Transform rewardTr;
    public GameObject rewardPref;
    public void SetReawrd(int n)
    {
        count = n;
        StartCoroutine(CoSetReward());
    }

    public IEnumerator CoSetReward()
    {
        while(count != 0)
        {
            Instantiate(rewardPref, rewardTr);
            count--;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
