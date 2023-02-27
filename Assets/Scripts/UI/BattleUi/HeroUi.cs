using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUi : MonoBehaviour
{
    public HeroSkill heroSkill;
    public Image heroImage;
    public Slider hpBar;

    [SerializeField]
    private BuffList viewBuffList;
    private List<HeroBuff> buffList = new();
    public UnitState heroState;

    CharacterDataBundle heroData;

    public void SetHeroInfo(CharacterDataBundle data)
    {
        heroData = data;
        //heroImage.sprite = GameManager.Instance.iconSprites[$"Icon_{heroData.data.name}"];
        SetHp(heroData.data.healthPoint);
    }

    private void Awake()
    {
        viewBuffList.SetList(ref buffList);
    }

    public void AddBuff()
    {
        viewBuffList.AddBuff();
    }

    public void OnClickHeroSkill()
    {
        if(heroState == UnitState.Battle)
            heroSkill.OnClickSkill();
    }

    public void OnClickPopUp()
    {
        viewBuffList.OnClickPopUp();
    }
    public void SetHp(int nowHp)
    {
        hpBar.value = Mathf.Max((float)nowHp / (float)heroData.data.healthPoint);
    }
}
