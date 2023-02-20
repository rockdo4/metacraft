using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class HeroInfo : MonoBehaviour
{
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI levelText;
    public Image portrait;

    private CharacterData baseData;
    private AsyncOperationHandle handle;
    private bool loadFlag = false;

    public void SetData(CharacterData data)
    {
        baseData = data;
        heroNameText.text = baseData.heroName;
        gradeText.text = baseData.grade;
        typeText.text = baseData.type;
        levelText.text = baseData.level;
        Addressables.LoadAssetAsync<Sprite>(baseData.heroName).Completed +=
            (AsyncOperationHandle<Sprite> obj) =>
            {
                handle = obj;
                portrait.sprite = obj.Result;
                loadFlag = true;
            };
    }

    private void OnDisable()
    {
        if (loadFlag)
            Addressables.Release(handle);
        loadFlag = false;
    }
}