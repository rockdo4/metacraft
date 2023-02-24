using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroSkill : MonoBehaviour
{
    [SerializeField]
    private Image skillCoolDownImage;
    private float CoolDownFill {
        set {
            skillCoolDownImage.fillAmount = value;
        }
    }
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
        this.effect = effect;

        coolDownTimer = 0;
        CoolDownFill = coolDownTimer / coolDown;
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
            coolDownTimer = Mathf.Max(coolDownTimer, 0);
            CoolDownFill = coolDownTimer / coolDown;
        }
    }
    public void OnClickSkill()
    {
        if (IsCoolDown)
        {
            effect();
            CoolDownFill = 1;
            coolDownTimer = coolDown;
        }
    }
}
