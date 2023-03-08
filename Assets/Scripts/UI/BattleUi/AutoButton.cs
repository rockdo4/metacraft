using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoButton : MonoBehaviour
{
    public TextMeshProUGUI stateTxt;
    Dictionary<bool, string> buttonText = new();
    bool nowState = false;

    private void Awake()
    {
        buttonText[true] = "TRUE";
        buttonText[false] = "!TRUE";
    }
    public void ResetData()
    {
        SetAutoState(GameManager.Instance.playerData.isAuto);
    }
    public void SetAutoState(bool state)
    {
        nowState = state;
        stateTxt.text = buttonText[nowState];

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
