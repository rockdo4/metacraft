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
        get { return isSilence;   }
        set {
            isSilence = value;
            silenceImage.gameObject.SetActive(value);
        }
    }

    public void SetHeroInfo(CharacterDataBundle data)
    {
        heroData = data;
        heroImage.sprite = GameManager.Instance.GetSpriteByAddress($"Icon_{heroData.data.name}");
        SetHp(heroData.data.currentHp, heroData.data.healthPoint);
    }


    public BuffIcon AddIcon(BuffType type, float duration, int idx)
    {
        return viewBuffList.AddIcon(type, duration, idx);
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
        hpBar.value = nowHp / maxHp; // Mathf.Max((float)nowHp / maxHp); max 왜 쓴거임? - 진석 
                                            // 그러게요..? 여기다 쓸 이유가 없는데.. 최대체력 넘지 못하게 하는 코드
                                            //  작업하면서 무지성으로 복붙한거 같아요- 정연
    }
    public void SetCurrHp()
    {
        SetHp(heroData.data.currentHp, heroData.data.healthPoint);
    }

    public void SetDieImage()
    {
        dieImage.gameObject.SetActive(true);
    }
}
