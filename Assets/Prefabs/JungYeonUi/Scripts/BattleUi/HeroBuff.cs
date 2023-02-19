using System;
using UnityEngine;

public class HeroBuff : MonoBehaviour
{
    public float duration; //�� ���ӽð�
    public Action OnEnd;
    public GameObject popUpBuff;

    private void FixedUpdate()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            EndBuff();
    }

    public void EndBuff()
    {
        if (OnEnd != null)
            OnEnd();

        Destroy(popUpBuff);
        Destroy(gameObject);
    }

}
