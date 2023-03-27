using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialButton> tutorialButtonList = new();
    private int currChatWindowIndex = 0;
    private int chatLine = 0;

    private void Start()
    {
        if (!GameManager.Instance.playerData.isTutorial)
        {
            
        }
    }

    public void OnChatWindow(int index)
    {
        currChatWindowIndex = index;
        tutorialButtonList[index].OnWindow();
    }

    public void OffChatWindow()
    {
        tutorialButtonList[currChatWindowIndex].OffWindow();
    }

    public void OnNextChatLine()
    {
        // ���⼭ ä�� �� ����
        //tutorialButtonList[currChatWindowIndex].SetText(string);
        chatLine++;
    }

    public void OnSkip()
    {
        // ��� �� ��ŵ �������� �̵�
        // ������ ����ϴ� UI�� �ε��� ����
    }
}
