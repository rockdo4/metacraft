using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroSkill : MonoBehaviour
{
    [SerializeField]
    private Image skillCoolDownImage;
    [SerializeField]
    private GameObject skillActivedHighlight;
    [SerializeField]
    private GameObject skillActivedPanel;
    [SerializeField]
    //private TextMeshProUGUI skillDescriptionText;

    private float prevTimeScale ;
    public bool isAuto;

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

        prevTimeScale = BattleSpeed.Instance.GetSpeed;
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
    private void Update()
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
    public void SetCoolTime(float sec)
    {
        coolDownTimer = sec;
    }

    public void OnDownSkill()
    {
        if (IsCoolDown)
        {            
            SetActiveSkillGUIs(true);
            isPointerInSkillActivePanel = false;

            prevTimeScale = BattleSpeed.Instance.GetSpeed;          
            Time.timeScale = 0.25f;
        }
    }
    public void CancleSkill()
    {
        cancle();
        SetActiveSkillGUIs(false);
        Time.timeScale = BattleSpeed.Instance.GetSpeed;
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
        Time.timeScale = BattleSpeed.Instance.GetSpeed;
        CoolDownFill = 1;
        coolDownTimer = coolDown;
    }
    public IEnumerator OnAutoSkillActive(CharacterSkill skill)
    {
        OnDownSkill();

        yield return new WaitForSeconds(0.5f * Time.timeScale);

        if (isAuto)
        {
            CoolDownFill = 1;
            coolDownTimer = coolDown;
            ready();
            action();
            SetActiveSkillGUIs(false);
            Time.timeScale = BattleSpeed.Instance.GetSpeed;
        }
        else
        {
            cancle();
            SetActiveSkillGUIs(false);
            Time.timeScale = BattleSpeed.Instance.GetSpeed;
        }
    }

    private void SetActiveSkillGUIs(bool active)
    {
        skillActivedHighlight.SetActive(active);
        skillActivedPanel.SetActive(active);
        //skillDescriptionText.text = skillDescription;
    }

    public void IsPointerInSkillActivePanel(bool isIn)
    {
        if (isAuto)
        {
            offAreaIndicator();
            return;
        }

        isPointerInSkillActivePanel = isIn;

        if (!skillActivedHighlight.activeSelf)
            return;

        if (isIn)
            ready();
        else
            offAreaIndicator();
    }
}
