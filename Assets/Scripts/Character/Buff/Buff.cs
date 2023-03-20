using System;
using UnityEngine;

public class Buff
{
    public BuffInfo buffInfo;

    public AttackableUnit myCharacter;
    public Action<Buff> removeBuff;
    public Action endEvent;
    public BuffIcon icon;
    public float timer = 0;
    public float sec = 1f;

    public Buff(BuffInfo info, AttackableUnit character, Action<Buff> remove, BuffIcon icon = null, Action endEvent = null)
    {
        buffInfo = info;
        myCharacter = character;
        removeBuff = remove;
        this.icon = icon;
        this.endEvent = endEvent;
        timer = buffInfo.duration;
    }
    public void OnEffect()
    {

    }
    public void TimerUpdate()
    {
        if (buffInfo.inf)
            return;

        timer -= Time.deltaTime;
        if (icon != null)
        {
            sec -= Time.deltaTime;
            if (sec < 0)
            {
                sec = 1f;
                icon.count.text = ((int)timer).ToString();
            }
        }

        if (timer <= 0)
        {
            removeBuff?.Invoke(this);
            endEvent?.Invoke();
        }
    }
}


