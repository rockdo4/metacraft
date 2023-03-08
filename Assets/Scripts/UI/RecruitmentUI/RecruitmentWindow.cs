using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecruitmentWindow : MonoBehaviour
{
    public Button[] buttons;
    public PlayerData playerData;

    public GameObject currentRecruitmentButton;

    private void Start()
    {
        playerData = GameManager.Instance.playerData;

        if (playerData.officeLevel<3)
        {
            var count = 2;
            for (int i = buttons.Length-1; i > count; i--)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = null;
                buttons[i].interactable = false;
            } 
        }
        else
        {

        }
    }

    public void ClickRecruitmentButton()
    {
        currentRecruitmentButton = EventSystem.current.currentSelectedGameObject;
    }
}
