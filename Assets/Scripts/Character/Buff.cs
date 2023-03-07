using System;
using UnityEngine;

public class Buff
{
    public float duration; //�� ���ӽð�
    public BuffType type;
    public float buffScale;
    public BuffIcon icon;

    public Action<Buff> removeBuff;

    public void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            removeBuff(this);
    }
}


