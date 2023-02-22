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
    //TEST��

    private void Start()
    {
        UpdateMissionInfo();
    }

    public void UpdateMissionInfo()
    {
        Addressables.LoadAssetAsync<Sprite>("��ƿ����").Completed += (AsyncOperationHandle<Sprite> obj) => { portrait.sprite = obj.Result; };
        explanation.text = $"�̰��� ������Դϴ�.";
        fitProperties1.text = $"���� �Ӽ�1";
        fitProperties2.text = $"���� �Ӽ�2";
        fitProperties3.text = $"���� �Ӽ�3";
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
