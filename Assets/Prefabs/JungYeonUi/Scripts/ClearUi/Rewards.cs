using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewards : MonoBehaviour
{
    [SerializeField]
    private int count;

    public Transform rewardTr;
    public GameObject rewardPref;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SetReward());
        }
    }
    public IEnumerator SetReward()
    {
        while(count != 0)
        {
            Instantiate(rewardPref, rewardTr);
            count--;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
