using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static int textIndex = 0;                               // 가져와야 하는 텍스트들 인덱스
    public int TestIndex {      //임시로 넣을게,
        set {
            textIndex = value;
        }
    }

    public List<TutorialButton> tutorialButtonList = new();
    public static int currChatWindowIndex = 0;
    private int chatLine = 0;
    public BattleManager btMgr;
    private int startChatSkipIndex = 10;

    public Button skipButton;
    public TutorialMask tutorialMask;

    private List<List<string>> tutorialDialouges;
    private string keyHead = "tutorial_string_";
    public string[] keyTail;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
        ParseEventTable();
        OffAllTutorialButton();
        if (GameManager.Instance.playerData.isTutorial)
        {
            OnNextChatLine();
        }
        //if (btMgr != null)
        //{
        //    textIndex = startChatSkipIndex + 1;
        //    //currChatWindowIndex = 0;
        //}
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
        //if (currChatWindowIndex == 24)
        //    gm.playerData.isTutorial = false;
        if (currChatWindowIndex == 25)
        {
            Logger.Debug("Tutorial Clear");
            return;
        }

        if (!gm.playerData.isTutorial)
            return;

        if (btMgr != null)
            Time.timeScale = 0;

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
        if (tutorialButtonList[currChatWindowIndex].isMask)
        {
            tutorialMask.Setting(tutorialButtonList[currChatWindowIndex].GetComponent<Button>());
        }
        chatLine++;

        OnChatWindow(currChatWindowIndex);
        Logger.Debug($"{currChatWindowIndex} / {chatLine} / {tutorialDialouges[textIndex].Count}");
        if ((currChatWindowIndex >= 11 && currChatWindowIndex < 14) ||
            (currChatWindowIndex >= 2 && currChatWindowIndex < 7) ||
            currChatWindowIndex == 8 ||
            (currChatWindowIndex >= 18 && currChatWindowIndex < 21) ||
            (currChatWindowIndex >= 22 && currChatWindowIndex < 24))
            currChatWindowIndex++;
    }

    public void MoveNextChatWindow()
    {
        chatLine = 0;
        textIndex++;
        currChatWindowIndex++;
        OffChatWindow();
        OffAllTutorialButton();
    }

    public void OnNextChat()
    {
        MoveNextChatWindow();
        OnNextChatLine();
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
        if (Input.GetKeyDown(KeyCode.N))
        {
            OnNextChatLine();
        }
    }
}