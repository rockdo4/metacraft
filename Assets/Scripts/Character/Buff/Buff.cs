using System;
using UnityEngine;

public class Buff
{
    public BuffInfo buffInfo;

    public AttackableUnit myCharacter;
    public Action<Buff> removeBuff;
    public BuffIcon icon;
    public float timer = 0;

    public Buff(BuffInfo info, AttackableUnit character, Action<Buff> remove, BuffIcon icon = null)
    {
        buffInfo = info;
        myCharacter = character;
        removeBuff = remove;
        this.icon = icon;
        timer = buffInfo.duration;
    }
    public void OnEffect()
    {

    }
    public void TimerUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            removeBuff(this);
    }
}


