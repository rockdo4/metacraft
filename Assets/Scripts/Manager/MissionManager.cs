using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI explanation;
    public Button heroSlot1;
    public Button heroSlot2;
    public Button heroSlot3;
    public TextMeshProUGUI fitProperties1;
    public TextMeshProUGUI fitProperties2;
    public TextMeshProUGUI fitProperties3;
    public TextMeshProUGUI deductionAP;
    public TextMeshProUGUI ProperCombatPower;

    public GameObject heroSlectWindowPrefab;
    //TEST용

    private void Start()
    {
        UpdateMissionInfo();
    }

    public void UpdateMissionInfo()
    {
        Addressables.LoadAssetAsync<Sprite>("노틸러스").Completed += (AsyncOperationHandle<Sprite> obj) => { portrait.sprite = obj.Result; };
        explanation.text = $"이것은 설명란입니다.";
        fitProperties1.text = $"적합 속성1";
        fitProperties2.text = $"적합 속성2";
        fitProperties3.text = $"적합 속성3";
        deductionAP.text = $"AP - {10}";
        ProperCombatPower.text = $"0100/1000";
    }

    public void OnClickHeroSlot1Button()
    {
        heroSlectWindowPrefab.SetActive(true);
    }

    public void OnClickHeroSlot2Button()
    {
        heroSlectWindowPrefab.SetActive(true);
    }

    public void OnClickHeroSlot3Button()
    {
        heroSlectWindowPrefab.SetActive(true);
    }

    public void OnClickHeroWindowCloseButton()
    {
        heroSlectWindowPrefab.SetActive(false);
    }
}
