using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBuff : MonoBehaviour
{
    public float duration; //ÃÑ Áö¼Ó½Ã°£
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
