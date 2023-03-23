using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssetLoadProgress : MonoBehaviour
{
    public Image background;
    public Image fill;
    public TextMeshProUGUI text;
    public GameObject loadSceneButton;

    public void SetProgress(int count, int total)
    {
        float value = (float)count / total;
        fill.fillAmount = value;
        text.text = $"{value * 100f:0.00}%";
    }

    public void CompleteProgress()
    {
        text.text = $"{GameManager.Instance.GetStringByTable("title_load_complete")}";
        fill.fillAmount = 1;
        loadSceneButton.SetActive(true);
    }
}