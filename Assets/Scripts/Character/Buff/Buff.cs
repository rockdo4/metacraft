using System;
using UnityEngine;

[Serializable]
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
        if(!info.inf)
            icon.count.text = ((int)timer).ToString();
    }
    public void OnEffect()
    {

    }
    public void TimerUpdate()
    {
        if (buffInfo.inf)
            return;

        if (icon != null)
        {
            sec -= Time.deltaTime;
            if (sec <= 0)
            {
                sec = 1f;
                timer -= 1;
                icon.count.text = ((int)timer).ToString();
                icon.popUpBuff.count.text = icon.count.text;
            }
        }

        if (timer <= 0)
        {
            removeBuff?.Invoke(this);
            endEvent?.Invoke();
        }
    }
}


