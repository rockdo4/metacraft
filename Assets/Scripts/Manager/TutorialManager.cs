using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private static int textIndex = 0;                               // 가져와야 하는 텍스트들 인덱스

    public List<TutorialButton> tutorialButtonList = new();
    private int currChatWindowIndex = 0;
    private int chatLine = 0;
    public BattleManager btMgr;
    private int startChatSkipIndex = 8;

    public Button skipButton;
    public TutorialMask tutorialMask;

    private List<List<string>> tutorialDialouges;
    private string keyHead = "tutorial_string_";
    public string[] keyTail;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
        currChatWindowIndex = 0;
        ParseEventTable();
        OffAllTutorialButton();
        if (GameManager.Instance.playerData.isTutorial)
        {
            OnNextChatLine();
        }
        if (btMgr != null)
        {
            textIndex = startChatSkipIndex + 1;
        }
    }

    public void OnChatWindow(int index)
    {
        if (tutorialButtonList[index].chatWindow == tutorialButtonList[currChatWindowIndex])
        {
            tutorialButtonList[index].OnOutline();
            return;
        }

        OffAllTutorialButton();
        currChatWindowIndex = index;
        tutorialButtonList[index].OnWindow();
        tutorialButtonList[index].OnOutline();
    }

    public void OffChatWindow()
    {
        tutorialButtonList[currChatWindowIndex].OffWindow();
    }

    public void OnNextChatLine()
    {
        if (!gm.playerData.isTutorial)
            return;

        int count = tutorialDialouges[textIndex].Count;
        if (chatLine >= count)
        {
            chatLine = 0;
            textIndex++;
            currChatWindowIndex++;
            OffChatWindow();
            OffAllTutorialButton();
            return;
        }

        var chat = tutorialDialouges[textIndex][chatLine];
        tutorialButtonList[currChatWindowIndex].SetText(chat);
        chatLine++;

        OnChatWindow(currChatWindowIndex);
        Logger.Debug($"{currChatWindowIndex} / {chatLine} / {tutorialDialouges[textIndex].Count}");
        if (currChatWindowIndex >= 9 && currChatWindowIndex < 12)
            currChatWindowIndex++;
    }

    public void OnClickSkip()
    {
        OffSkipButton();
        chatLine = 0;
        OffChatWindow();
        if (textIndex < startChatSkipIndex)
            currChatWindowIndex = startChatSkipIndex;
        else
            currChatWindowIndex++;

        if (textIndex < 4)
            textIndex = 4;
        else
            textIndex++;
    }
    public void SetDefaltTimeScale()
    {
        Time.timeScale = 1f;
    }

    // 히어로들 ready 해주기
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
        if (skipButton != null)
            skipButton.gameObject.SetActive(false);
    }
    public void OffAllTutorialButton()
    {
        for (int i = 0; i < tutorialButtonList.Count; i++)
        {
            tutorialButtonList[i].OffWindow();
            tutorialButtonList[i].OffOutline();
        }
    }
    private void ParseEventTable()
    {
        GameManager gm = GameManager.Instance;

        tutorialDialouges = new(keyTail.Length);

        for (int i = 0; i < tutorialDialouges.Capacity; i++)
        {
            tutorialDialouges.Add(new(10));
            int dialougeNum = 1;
            while (dialougeNum < 100)
            {
                var key = $"{keyHead}{keyTail[i]}{dialougeNum}";
                var dialouge = gm.GetStringByTable(key);
                if (dialouge.Equals(key.ToLower()))
                    break;

                tutorialDialouges[i].Add(dialouge);
                dialougeNum++;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnClickSkip();
        }
    }
}