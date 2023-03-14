using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoButton : MonoBehaviour
{
    public Image butonImage;
    public TextMeshProUGUI stateTxt;
    Color[] textColors = { new Color(0.2f, 0.2f, 0.2f), Color.white };
    Color[] burronColors = { new Color(0.5f, 0.5f, 0.5f), Color.green };
    bool nowState = false;
    int Idx => !nowState ? 0 : 1;

    public void ResetData()
    {
        SetAutoState(GameManager.Instance.playerData.isAuto);
    }
    public void SetAutoState(bool state)
    {
        nowState = state;
        butonImage.color = burronColors[Idx];
        stateTxt.color = textColors[Idx];

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
