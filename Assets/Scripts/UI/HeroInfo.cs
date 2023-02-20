using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class HeroInfo : MonoBehaviour, IUseAddressable
{
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI levelText;
    public Image portrait;

    private CharacterData baseData;
    private AsyncOperationHandle handle;
    private bool loadFlag = false;

    public AsyncOperationHandle Handle
    {
        get => handle;
        set => handle = value;
    }
   
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

    public void SetData(CharacterData data)
    {
        baseData = data;
        heroNameText.text = baseData.heroName;
        gradeText.text = baseData.grade;
        typeText.text = baseData.type;
        levelText.text = baseData.level;
        LoadAddressable(baseData.heroName);
    }

    private void OnDisable()
    {
        if (loadFlag)
            ReleaseAddressable();
        loadFlag = false;
    }
}