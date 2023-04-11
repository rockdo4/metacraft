using UnityEngine;
using UnityEngine.UI;

public class HeroUi : MonoBehaviour
{
    public HeroSkill heroSkill;
    public Image heroImage;
    public Slider hpBar;
    public Image dieImage;
    public Image silenceImage;

    [SerializeField]
    private BuffList viewBuffList;
    public UnitState heroState;

    public CharacterDataBundle heroData;
    public float unitHpScale;
    public bool isAuto;
    public void SetAuto(ref bool state) => isAuto = state;
    public bool isSilence;
    public bool IsSilence {
        get { return isSilence; }
        set {
            isSilence = value;
            silenceImage.gameObject.SetActive(value);
        }
    }

    public void SetHeroInfo(AttackableHero data)
    {
        heroData = data.GetUnitData();
        string key = $"icon_{heroData.data.name}";
        heroImage.sprite = GameManager.Instance.GetSpriteByAddress(key);
        SetHp(data.UnitHpScale);
    }

    public BuffIcon AddIcon(BuffType type, float duration, int idx, Sprite sprite)
    {
        return viewBuffList.AddIcon(type, duration, idx, sprite);
    }

    public void RemoveBuff(BuffIcon icon)
    {
        viewBuffList.RemoveBuff(icon);
    }

    public void OnClickHeroSkill()
    {
        if (heroState == UnitState.Battle && !isAuto) 
        {
            heroSkill.OnDownSkill();
        }
    }

    public void OnClickPopUp()
    {
        viewBuffList.OnClickPopUp();
    }

    public void SetHp(float nowHp, float maxHp)
    {
        hpBar.value = (float)(nowHp / maxHp);
    }
    public void SetHp(float value)
    {
        hpBar.value = value;
    }

    public void SetDieImage()
    {
        dieImage.gameObject.SetActive(true);
    }
}