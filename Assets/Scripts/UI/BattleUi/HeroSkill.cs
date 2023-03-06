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

    private float prevTimeScale ;

    private bool isPointerInSkillActivePanel;
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
    public Action cancle;

    public void Set(float coolDown, Action effect, Action action, Action cancle)
    {
        this.coolDown = coolDown;
        this.effect = effect;
        this.action = action;
        this.cancle = cancle;

        prevTimeScale = GameObject.FindObjectOfType<BattleSpeed>().GetSpeed;
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
            SetActiveSkillGUIs(true);
            isPointerInSkillActivePanel = false;

            prevTimeScale = Time.timeScale;
            Time.timeScale = 0.25f;
        }
    }
    public void CancleSkill()
    {
        Time.timeScale = prevTimeScale;
        cancle();
        SetActiveSkillGUIs(false);
        Time.timeScale = prevTimeScale;
    }
    public void OnUpSkillActive()
    {
        if (!skillActivedHighlight.activeSelf)
            return;

        if (!isPointerInSkillActivePanel)
        {
            CancleSkill();
            return;
        }

        action();
        SetActiveSkillGUIs(false);
        Time.timeScale = prevTimeScale;
        CoolDownFill = 1;
        coolDownTimer = coolDown;
    }

    private void SetActiveSkillGUIs(bool active)
    {
        skillActivedHighlight.SetActive(active);
        skillActivedPanel.SetActive(active);
    }

    public void IsPointerInSkillActivePanel(bool isIn)
    {
        isPointerInSkillActivePanel = isIn;

        if (!skillActivedHighlight.activeSelf)
            return;

        if (isIn)
            effect();
        else
            cancle();
    }
}
