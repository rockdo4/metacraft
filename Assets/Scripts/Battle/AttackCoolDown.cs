using System;
using UnityEngine;

public class AttackCoolDown : MonoBehaviour
{
    public FindTarget target;

    public bool IsCoolDown {
        get {
            return coolDownTimer <= 0;
        }
    }
    private float coolDown;
    private float coolDownTimer;

    public Action effect;

    public void Set(float coolDown, Action effect)
    {
        this.coolDown = coolDown;
        coolDownTimer = coolDown;
        this.effect = effect;
    }

    private void FixedUpdate()
    {
        CoolDownUpdate();
    }

    public void CoolDownUpdate()
    {
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
            coolDownTimer = Mathf.Max(0, coolDownTimer);
        }
        else if(target.IsTarget)
        {
            coolDownTimer = coolDown;
            effect();
        }
    }
}
