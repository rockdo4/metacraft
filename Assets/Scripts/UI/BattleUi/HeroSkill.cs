using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroSkill : MonoBehaviour
{
    [SerializeField]
    private Image skillCoolDownImage;
    [SerializeField]
    private GameObject skillActivedHighlight;
    [SerializeField]
    private GameObject skillActivedPanel;
    [SerializeField]
    private TextMeshProUGUI skillDescriptionText;

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

    private string skillDescription;

    public Action ready;
    public Action action;
    public Action offAreaIndicator;
    public Action cancle;

    public void Set(float coolDown, string skillDescription)
    {
        this.coolDown = coolDown;
        this.skillDescription = skillDescription;

        prevTimeScale = GameObject.FindObjectOfType<BattleSpeed>().GetSpeed;
        coolDownTimer = 0;
        CoolDownFill = coolDownTimer / coolDown;
    }
    public void SetActions(Action ready, Action action, Action offAreaIndicator,Action cancle)
    {
        this.ready = ready;
        this.action = action;
        this.offAreaIndicator = offAreaIndicator;
        this.cancle = cancle;
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

            prevTimeScale = GameObject.FindObjectOfType<BattleSpeed>().GetSpeed;
            Time.timeScale = 0.25f;
        }
    }
    public void CancleSkill()
    {
        Time.timeScale = GameObject.FindObjectOfType<BattleSpeed>().GetSpeed;
        cancle();
        SetActiveSkillGUIs(false);
        Time.timeScale = GameObject.FindObjectOfType<BattleSpeed>().GetSpeed;
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
        Time.timeScale = GameObject.FindObjectOfType<BattleSpeed>().GetSpeed;
        CoolDownFill = 1;
        coolDownTimer = coolDown;
    }
    public void OnAutoSkillActive()
    {
        ready();
        action();
        SetActiveSkillGUIs(false);
        Time.timeScale = GameObject.FindObjectOfType<BattleSpeed>().GetSpeed;
        CoolDownFill = 1;
        coolDownTimer = coolDown;
    }

    private void SetActiveSkillGUIs(bool active)
    {
        skillActivedHighlight.SetActive(active);
        skillActivedPanel.SetActive(active);
        skillDescriptionText.text = skillDescription;
    }

    public void IsPointerInSkillActivePanel(bool isIn)
    {
        isPointerInSkillActivePanel = isIn;

        if (!skillActivedHighlight.activeSelf)
            return;

        if (isIn)
            ready();
        else
            offAreaIndicator();
    }
}
