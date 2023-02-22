using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class BattleHero : MonoBehaviour
{
    public HeroSkill heroSkill;
    public Image heroImage;
    public Slider hpBar;

    [SerializeField]
    private BuffList viewBuffList;
    private List<HeroBuff> buffList = new();
    public UnitState heroState;

    HeroData heroData;

    public void SetHeroInfo(HeroData data)
    {
        heroData = data;
        Addressables.LoadAssetAsync<Sprite>(heroData.info.resourceAddress).Completed +=
            (AsyncOperationHandle<Sprite> obj) =>
            {
                heroImage.sprite = obj.Result;
            };
        SetHp(heroData.stats.healthPoint);
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
        hpBar.value = Mathf.Max((float)nowHp / (float)heroData.stats.healthPoint);
    }
}
