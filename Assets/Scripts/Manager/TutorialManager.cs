using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialButton> tutorialButtonList = new();
    private int currChatWindowIndex = 0;
    private int chatLine = 0;
    public BattleManager btMgr;
    public TutorialMask tutorialMask;

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
        // 여기서 채팅 줄 관리
        //tutorialButtonList[currChatWindowIndex].SetText(string);
        chatLine++;
    }

    public void OnSkip()
    {
        // 대사 줄 스킵 구간까지 이동
        // 다음에 출력하는 UI들 인덱스 설정
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
}
