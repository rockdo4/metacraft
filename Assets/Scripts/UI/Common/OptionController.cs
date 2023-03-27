using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;

    public Image countryIcon;
    public TextMeshProUGUI countryText;

    public Sprite[] countryIcons;
    public string[] countryTexts;

    private void Awake()
    {
        bgmSlider.value = AudioManager.Instance.bgm;
        seSlider.value = AudioManager.Instance.se;        
        SetLanguage(GameManager.Instance.LanguageIndex);
    }
    private void SetLanguage(int index)
    {
        countryIcon.sprite = countryIcons[index];
        countryText.text = countryTexts[index];
    }
    public void SetBGMVolume(float volume)
    {
        AudioManager.Instance.SetBGMVolume(volume);
    }
    public void SetSEVolume(float volume)
    {
        AudioManager.Instance.SetSEVolume(volume);
    }

}
