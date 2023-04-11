using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetNickname : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI buttonText;
    public TMP_InputField inputField;
    public Button confirmButton;

    private void Start()
    {
        GameManager gm = GameManager.Instance;
        title.text = gm.GetStringByTable("set_nickname_title");
        buttonText.text = gm.GetStringByTable("set_nickname_confirm");
    }

    public void ChangeValue(string value)
    {
        confirmButton.interactable = (value.Length == 0) ? false : true;
        GameManager.Instance.playerData.playerName = value;
    }
}