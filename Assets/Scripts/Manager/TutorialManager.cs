using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private List<List<string>> tempTexts;
    private static int textIndex = 0;                               // �����;� �ϴ� �ؽ�Ʈ�� �ε���

    public List<TutorialButton> tutorialButtonList = new();
    private int currChatWindowIndex = 0;
    private int chatLine = 0;
    public BattleManager btMgr;
    private int startChatSkipIndex = 2;

    public Button skipButton;

    private void Start()
    {
        if (!GameManager.Instance.playerData.isTutorial)
        {
            
        }
        if (btMgr != null)
        {
            textIndex = 3;
        }
    }

    public void OnChatWindow(int index)
    {
        OffAllTutorialButton();
        currChatWindowIndex = index;
        tutorialButtonList[index].OnWindow();
    }

    public void OffChatWindow()
    {
        tutorialButtonList[currChatWindowIndex].OffWindow();
    }

    public void OnNextChatLine()
    {
        chatLine++;
        if (chatLine == tempTexts[textIndex].Count)
        {
            chatLine = 0;
            textIndex++;
            OffChatWindow();
            return;
        }

        var chat = tempTexts[textIndex][chatLine];
        tutorialButtonList[currChatWindowIndex].SetText(chat);
        OnChatWindow(currChatWindowIndex);
    }

    public void OnClickSkip()
    {
        OffSkipButton();
        // ��� �� ��ŵ �������� �̵�
        // ������ ����ϴ� UI�� �ε��� ����
        chatLine = 0;

        if (textIndex < startChatSkipIndex)
            textIndex = startChatSkipIndex;
        else
            textIndex++;

        OffChatWindow();
    }
    public void SetDefaltTimeScale()
    {
        Time.timeScale = 1f;
    }

    // ����ε� ready ���ֱ�
    public void SetBattleSceneHeroesReady()
    {
        btMgr.SetHeroesReady();
    }

    public void OnSkipButton()
    {
        skipButton.gameObject.SetActive(true);
    }
    public void OffSkipButton()
    {
        skipButton.gameObject.SetActive(false);
    }
    public void OffAllTutorialButton()
    {
        for (int i = 0; i < tutorialButtonList.Count; i++)
        {
            tutorialButtonList[i].OffWindow();
        }
    }
}