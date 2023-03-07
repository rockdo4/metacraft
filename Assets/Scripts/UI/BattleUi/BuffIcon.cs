using System;
using UnityEngine;

public class BuffIcon : MonoBehaviour
{
    public BuffType buffType;
    public float duration; //�� ���ӽð�

    public Action OnEnd;
    public GameObject popUpBuff;

    //private void FixedUpdate()
    //{
    //    duration -= Time.deltaTime;
    //    if (duration <= 0)
    //        EndBuff();
    //}

    public void EndBuffIcon()
    {
        if (OnEnd != null)
            OnEnd();

        Destroy(popUpBuff);
        Destroy(gameObject);
    }

}
