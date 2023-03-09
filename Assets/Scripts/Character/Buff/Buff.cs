using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Buff/info")]
public class Buff : ScriptableObject
{
    public string id;
    public AttackableUnit myCharacter;
    public float duration; //ÃÑ Áö¼Ó½Ã°£
    public BuffType type;
    public float buffScale;
    public BuffIcon icon;
    public bool inf = false;
    public Effect buffParticle;

    public Action<Buff> removeBuff;

    public void OnEffect()
    {

    }
    public void TimerUpdate()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            removeBuff(this);
    }
}


