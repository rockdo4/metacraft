using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoButton : MonoBehaviour
{
    public Image butonImage;
    public TextMeshProUGUI stateTxt;
    Dictionary<bool, Color> textColor = new();
    Dictionary<bool, Color> burronColor = new();
    bool nowState = false;

    private void Init()
    {
        textColor[true] = Color.green;
        burronColor[true] = Color.white;

        textColor[false] = new Color(0.2f, 0.2f, 0.2f);
        burronColor[false] = new Color(0.5f, 0.5f, 0.5f);
    }

    public void ResetData()
    {
        SetAutoState(GameManager.Instance.playerData.isAuto);
    }
    public void SetAutoState(bool state)
    {
        if (textColor.Count == 0 || burronColor.Count == 0)
        {
            Init();
        }

        nowState = state;
        butonImage.color = burronColor[nowState];
        stateTxt.color = textColor[nowState];

        var characters = GameObject.FindObjectsOfType<AttackableUnit>();

        foreach(var character in characters)
        {
            character.IsAuto = nowState;
        }
    }
    public void OnClick()
    {
        SetAutoState(!nowState);
        GameManager.Instance.playerData.isAuto = nowState;
    }
}
