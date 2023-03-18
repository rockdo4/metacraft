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

    public void ResetData(List<AttackableUnit> heroList)
    {
        SetAutoState(GameManager.Instance.playerData.isAuto, heroList);
    }
    public void SetAutoState(bool state, List<AttackableUnit> heroList = null)
    {
        nowState = state;
        butonImage.color = burronColors[Idx];
        stateTxt.color = textColors[Idx];

        if (heroList != null)
        {
            foreach (var character in heroList)
            {
                character.IsAuto = nowState;
            }
        }
    }
    public void OnClick()
    {
        SetAutoState(!nowState);
        GameManager.Instance.playerData.isAuto = nowState;
    }
}
