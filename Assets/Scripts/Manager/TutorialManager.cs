using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private Color outlineColor;
    private Color outlineSettingColor;
    public List<GameObject> chatWindows;
    private List<TextMeshProUGUI> texts = new();
    private int currChatWindowIndex = 0;
    private int chatLine = 0;

    private void Start()
    {
        for (int i = 0; i < chatWindows.Count; i++)
        {
            texts.Add(chatWindows[i].GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    public void OnOutline(Outline outlinePrefab)
    {
        outlineColor = outlinePrefab.OutlineColor;
        outlinePrefab.OutlineColor = outlineSettingColor;
    }

    public void OffOutline(Outline outlinePrefab)
    {
        outlinePrefab.OutlineColor = outlineColor;
    }

    public void SetOutlineColor(Color color)
    {
        outlineSettingColor = color;
    }

    public void OnChatWindow(int index)
    {
        currChatWindowIndex = index;
        chatWindows[index].SetActive(true);
    }

    public void OffChatWindow()
    {
        chatWindows[currChatWindowIndex].SetActive(false);
    }

    public void OnNextChatLine()
    {
        // 여기서 채팅 줄 관리
        //texts[currChatWindowIndex].text = ~
    }

    public void OnSkip()
    {
        // 대사 줄 이동
        // 다음에 출력하는 UI들 인덱스 설정
    }
}
