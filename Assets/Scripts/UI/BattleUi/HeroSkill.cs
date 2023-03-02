using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroSkill : MonoBehaviour
{
    [SerializeField]
    private Image skillCoolDownImage;
    [SerializeField]
    private GameObject skillActivedHighlight;
    [SerializeField]
    private GameObject skillActivedPanel;

    bool isInskillActivedPanelArea = false;
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
    public Action action;

    public void Set(float coolDown, Action effect, Action action)
    {
        this.coolDown = coolDown;
        this.effect = effect;
        this.action = action;

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
    public void OnDownSkill()
    {
        if (IsCoolDown)
        {
            effect();
            SetActiveSkillGUIs(true);
        }
    }
    public void CancleSkill()
    {
        if (!skillActivedHighlight.activeSelf)
            return;

        effect();
        SetActiveSkillGUIs(false);
    }
    public void OnUpSkillActive()
    {
        if (!skillActivedHighlight.activeSelf)
            return;
        
        action();
        SetActiveSkillGUIs(false);
        CoolDownFill = 1;
        coolDownTimer = coolDown;
    }

    private void SetActiveSkillGUIs(bool active)
    {
        skillActivedHighlight.SetActive(active);
        skillActivedPanel.SetActive(active);
    }
    public void IsInskillActivedPanelArea(bool isIn)
    {
        isInskillActivedPanelArea = isIn;
        Logger.Debug(isIn);
    }
}
