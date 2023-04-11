using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private GameManager gm;
    private List<Dictionary<string, object>> tutorialDic;
    public List<TutorialEvent> textBoxes;
    public List<TutorialOutline> outlines;
    public GameObject skipButton;
    public GameObject dialoguePanel;

    public static int currEv = 0;
    private int outLineNumber = 0;
    private int textBoxNumber = 0;
    public bool btEnd = false;

    private static bool isOfficeTutorialComplete = false;
    private static bool isBattleTutorialComplete = false;
    private bool isFirstOfficeTutorial = false;
    private bool isStop = false;

    private void Start()
    {
        gm = GameManager.Instance;

        tutorialDic = gm.tutorialIndexTable;
        if (gm.playerData.isTutorial)
        {
            if (currEv != 14)
                OnEvent();

            if (!gm.playerData.isTutorialFirstEntry)
            {
                gm.inventoryData.AddItem("60300003", 2000);
                gm.inventoryData.AddItem("60300022", 30);
                gm.inventoryData.AddItem("60300001", 50000);
                gm.playerData.isTutorialFirstEntry = true;
            }
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
                var btMgr = FindObjectOfType<BattleManager>();
                if (btMgr != null)
                {
                    btMgr.ResetHeroes();
                }

                gm.LoadScene((int)SceneIndex.Office);
                isBattleTutorialComplete = true;
                currEv = i;
                break;
            }
        }
    }

    public void OnEvent()
    {
        if (!gm.playerData.isTutorial)
        {
            return;
        }

        outLineNumber = (int)tutorialDic[currEv]["OutLineNumber"];
        textBoxNumber = (int)tutorialDic[currEv]["ScriptNumber"];
        string currEvText = (string)tutorialDic[currEv]["StringCode"];

        if (isFirstOfficeTutorial && gm.currentScene == SceneIndex.Office)
        {
            isFirstOfficeTutorial = false;
            return;
        }

        if (gm.currentScene == SceneIndex.Battle)
        {
            if (isStop)
            {
                isStop = false;
                return;
            }
            if (currEv == 33)
            {
                return;
            }
        }

        string text = gm.GetStringByTable(currEvText);
        OnTextBox(textBoxNumber, text);
        OnSkipBox();


        if (outLineNumber != -1)
        {
            OnOutline(outLineNumber);

            if (currEv != 15)
                outlines[outLineNumber].AddEventOriginalButton(OnNextTutorialEvent);
            else
            {
                outlines[outLineNumber].AddEventOriginalButton(SetTextBoxActive);
                outlines[outLineNumber].AddEventOriginalButton(SetOutlineActive);
                return;
            }

            outlines[outLineNumber].AddEventOriginalButton(OnEvent);
        }
        else
        {
            if (currEv != 31)
                dialoguePanel.SetActive(true);
        }
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

    private void OnSkipBox()
    {
        if (currEv < 33)
        {
            skipButton.SetActive(true);
        }
        else
        {
            skipButton.SetActive(false);
        }
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

        if (gm.currentScene == SceneIndex.Battle)
        {
            if (currEv == 18 ||
                currEv == 21 ||
                currEv == 23 ||
                currEv == 29 ||
                currEv == 30)
            {
                isStop = true;
                return;
            }
        }

        currEv++;
        if (currEv >= tutorialDic.Count)
        {
            for (int i = 0; i < textBoxes.Count; i++)
            {
                textBoxes[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < outlines.Count; i++)
            {
                outlines[i].SetActiveOutline(false);
            }
            gm.playerData.isTutorial = false;

            var attackableHero = gm.heroDatabase[0].GetComponent<AttackableHero>();
            attackableHero.GetUnitData().data.level = 1;

            gm.SaveAllData();
            return;
        }

        if (currEv == 14 && gm.currentScene == SceneIndex.Office)
            isFirstOfficeTutorial = true;
    }

    public void SetOutlineButton(GameObject button, bool needAdjust = true)
    {
        int tempOutLineNumber = (int)tutorialDic[currEv + 1]["OutLineNumber"];

        if (outlines[tempOutLineNumber].originalButton == null)
        {
            outlines[tempOutLineNumber].originalButton = button;
            outlines[tempOutLineNumber].NeedAdjustPos = needAdjust;
        }
    }

    public void SetTextBoxActive()
    {
        textBoxes[textBoxNumber].gameObject.SetActive(false);
    }
    public void SetOutlineActive()
    {
        outlines[outLineNumber].gameObject.SetActive(false);
    }
}
