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

    public void SetHeroInfo(CharacterDataBundle data)
    {
        heroData = data;
        string key = $"icon_{heroData.data.name}";
        heroImage.sprite = GameManager.Instance.GetSpriteByAddress(key);
        SetHp();
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

    public void SetHp(float? nowHp = null, float? maxHp = null)
    {
        if (nowHp == null || maxHp == null)
            hpBar.value = heroData.data.currentHp / heroData.data.healthPoint;
        else
            hpBar.value = (float)(nowHp / maxHp);
    }


    public void SetDieImage()
    {
        dieImage.gameObject.SetActive(true);
    }
}