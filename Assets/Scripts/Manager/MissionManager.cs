using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI explanation;
    public TextMeshProUGUI ExpectedCost;
    public Button heroSlot1;
    public Button heroSlot2;
    public Button heroSlot3;
    public TextMeshProUGUI fitProperties1;
    public TextMeshProUGUI fitProperties2;
    public TextMeshProUGUI fitProperties3;
    public TextMeshProUGUI deductionAP;
    public TextMeshProUGUI ProperCombatPower;

    public GameObject heroSlectWindowPrefab;
    //TEST¿ë
    List<Dictionary<string, object>> testTable;

    private void Start()
    {
        testTable = CSVReader.Read("TestMissionInfoTable");
        UpdateMissionInfo();
    }

    public void UpdateMissionInfo()
    {
        Addressables.LoadAssetAsync<Sprite>(testTable[0]["BossID"]).Completed += (AsyncOperationHandle<Sprite> obj) => { portrait.sprite = obj.Result; };
        explanation.text = $"{testTable[0]["OperationDescription"]}";
        ExpectedCost.text = $"{testTable[0]["ExpectedCostID"]}";
        fitProperties1.text = $"{testTable[0]["FitProperties1"]}";
        fitProperties2.text = $"{testTable[0]["FitProperties2"]}";
        fitProperties3.text = $"{testTable[0]["FitProperties3"]}";
        deductionAP.text = $"AP -{testTable[0]["ConsumptionBehavior"]}";
        ProperCombatPower.text = $"1000/{testTable[0]["ProperCombatPower"]}";
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
