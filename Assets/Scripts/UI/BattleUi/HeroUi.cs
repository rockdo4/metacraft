using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUi : MonoBehaviour
{
    public HeroSkill heroSkill;
    public Image heroImage;
    public Slider hpBar;
    public Image dieImage;

    [SerializeField]
    private BuffList viewBuffList;
    private List<BuffIcon> buffList;
    public UnitState heroState;

    CharacterDataBundle heroData;

    public void SetHeroInfo(CharacterDataBundle data)
    {
        heroData = data;
        heroImage.sprite = GameManager.Instance.iconSprites[$"Icon_{heroData.data.name}"];
        SetHp(heroData.data.currentHp, heroData.data.healthPoint);
    }

    public void SetBuffList(ref List<BuffIcon> list)
    {
        buffList = list;
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
        if(heroState == UnitState.Battle)
            heroSkill.OnDownSkill();
    }

    public void OnClickPopUp()
    {
        viewBuffList.OnClickPopUp();
    }
    public void SetHp(int nowHp, int maxHp)
    {
        hpBar.value = Mathf.Max((float)nowHp / (float)maxHp);
    }

    public void SetDieImage()
    {
        dieImage.gameObject.SetActive(true);
    }
}
