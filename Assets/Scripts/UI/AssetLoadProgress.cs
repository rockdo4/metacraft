using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssetLoadProgress : MonoBehaviour
{
    public Image background;
    public Image fill;
    public TextMeshProUGUI text;
    public GameObject loadSceneButton;

    private void Awake()
    {
        fill.fillAmount = 0;
    }

    public void SetProgress(int count, int total)
    {
        text.text = $"{count} / {total}";
        fill.fillAmount = (float)count / total;
    }

    public void CompleteProgress()
    {
        text.text = $"{GameManager.Instance.GetStringByTable("title_load_complete")}";
        fill.fillAmount = 1;
        loadSceneButton.SetActive(true);
    }
}