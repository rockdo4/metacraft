using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class HeroInfoButton : MonoBehaviour
{
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI levelText;
    public Image portrait;

    private AsyncOperationHandle handle;
    private bool loadFlag = false;
   
    public void LoadAddressable(string address)
    {
        Addressables.LoadAssetAsync<Sprite>(address).Completed +=
            (AsyncOperationHandle<Sprite> obj) =>
            {
                handle = obj;
                loadFlag = true;
                portrait.sprite = obj.Result;
            };
    }

    public void ReleaseAddressable()
    {
        Addressables.Release(handle);
    }

    public void SetData(Hero data)
    {
        InfoHero info = data.info;
        heroNameText.text = info.name;
        gradeText.text = info.grade;
        jobText.text = info.job;
        levelText.text = info.level.ToString();
        LoadAddressable(info.resourceAddress);
    }

    private void OnDisable()
    {
        if (loadFlag)
            ReleaseAddressable();
        loadFlag = false;
    }
}