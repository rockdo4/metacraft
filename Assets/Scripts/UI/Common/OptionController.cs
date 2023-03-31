using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    public Toggle soundOnOff;    

    public Slider bgmSlider;
    public Slider seSlider;

    public Image countryIcon;
    public TextMeshProUGUI countryText;

    public Sprite[] countryIcons;
    public string[] countryTexts;

    static int testLanguageChangeIndex = 0;

    private void Awake()
    {
        bgmSlider.value = Mathf.Pow(10.0f, (AudioManager.Instance.bgm / 20.0f));
        seSlider.value = Mathf.Pow(10.0f, (AudioManager.Instance.se / 20.0f));
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

    public void OnOffSound(bool onOff)
    {
        AudioManager.Instance.OnOffSound(onOff);
    }
    public void ChangeLanguage()
    {
        //여기에 언어 변경 함수 추가        
        SetLanguage(testLanguageChangeIndex++ % 2);
    }

}
