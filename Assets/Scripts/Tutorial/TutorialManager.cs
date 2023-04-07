using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private GameManager gm;
    private List<Dictionary<string, object>> tutorialDic;
    public List<TutorialEvent> textBoxes;
    public List<TutorialOutline> outlines;

    private static int currEv = 0;
    private int outLineNumber = 0;

    private static bool isOfficeTutorialComplete = false;
    private static bool isBattleTutorialComplete = false;

    private void Start()
    {
        gm = GameManager.Instance;
        tutorialDic = gm.tutorialIndexTable;
        if (gm.playerData.isTutorial)
        {
            if (currEv != 14)
                OnEvent();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            OnNextTutorialEvent();
            OnEvent();
        }
    }

    public void OnClickSkip()
    {
        for (int i = currEv; i < tutorialDic.Count; i++)
        {
            bool skipSection = Convert.ToBoolean(tutorialDic[i]["IsSkipStop"]);
            if (i == 14 && !isOfficeTutorialComplete && skipSection)
            {
                    gm.ClearBattleGroups();
                    gm.battleGroups[0] = 0;
                    gm.currentSelectMission = gm.missionInfoDifficulty[0][0];
                    gm.LoadScene((int)SceneIndex.Battle);
                    isOfficeTutorialComplete = true;
                    currEv = i;
                    break;
            }
            else if (i == 33 && !isBattleTutorialComplete && skipSection)
            {
                gm.LoadScene((int)SceneIndex.Office);
                isBattleTutorialComplete = true;
                currEv = i;
                break;
            }
        }
    }

    public void OnEvent()
    {
        outLineNumber = (int)tutorialDic[currEv]["OutLineNumber"];
        int scriptNumber = (int)tutorialDic[currEv]["ScriptNumber"];
        string currEvText = (string)tutorialDic[currEv]["StringCode"];

        if (outLineNumber != -1)
        {
            OnOutline(outLineNumber);
            outlines[outLineNumber].AddEventOriginalButton(OnNextTutorialEvent);
            outlines[outLineNumber].AddEventOriginalButton(OnEvent);
        }

        string text = gm.GetStringByTable(currEvText);
        OnTextBox(scriptNumber, text);
    }

    private void OnTextBox(int index, string text)
    {
        for (int i = 0; i < textBoxes.Count; i++)
        {
            textBoxes[i].gameObject.SetActive(false);
        }

        textBoxes[index].gameObject.SetActive(true);
        textBoxes[index].textBox.text = text;
    }
    private void OnOutline(int index)
    {
        for (int i = 0; i < outlines.Count; i++)
        {
            outlines[i].SetActiveOutline(false);
        }

        outlines[index].SetActiveOutline(true);
    }

    public void OnNextTutorialEvent()
    {
        if (outLineNumber != -1)
        {
            outlines[outLineNumber].RemoveEventOriginalButton(OnEvent);
            outlines[outLineNumber].RemoveEventOriginalButton(OnNextTutorialEvent);
            outlines[outLineNumber].SetActiveOutline(false);
        }
        currEv++;
        if (currEv >= tutorialDic.Count)
        {
            return;
        }
    }

    public void SetOutlineButton(GameObject button)
    {
        if (outlines[outLineNumber].originalButton == null)
        {
            outlines[outLineNumber].originalButton = button;
        }

        // ��ġ ����
        var rect = outlines[outLineNumber].originalButton.GetComponent<RectTransform>();
        outlines[outLineNumber].SetRectTrPos(rect.anchoredPosition);
        outlines[outLineNumber].AddEventOriginalButton(OnNextTutorialEvent);
        outlines[outLineNumber].AddEventOriginalButton(OnEvent);
    }
}
