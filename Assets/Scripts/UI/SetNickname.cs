using TMPro;
using UnityEngine;

public class SetNickname : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI buttonText;
    public TMP_InputField inputField;
    //public string nickname;

    private void Start()
    {
        GameManager gm = GameManager.Instance;
        title.text = gm.GetStringByTable("set_nickname_title");
        buttonText.text = gm.GetStringByTable("set_nickname_confirm");
    }
}