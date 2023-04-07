using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private GameManager gm;
    private List<Dictionary<string, object>> tutorialDic; 
    public List<TutorialEvent> textBoxes;
    public List<TutorialOutline> outlines;

    public static int currEv = 0;
    private int outLineNumber = 0;
    public bool btEnd = false;

    private void Start()
    {
        gm = GameManager.Instance;
        tutorialDic = gm.tutorialIndexTable;
        if(gm.playerData.isTutorial)
        {
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
            bool skipSection = (bool)tutorialDic[i]["IsSkipStop"];
            if (skipSection)
            {
                if (i == 15)
                {
                    gm.ClearBattleGroups();
                    gm.battleGroups[0] = 0;
                    gm.LoadScene((int)SceneIndex.Battle);
                    return;
                }
                currEv = i;
                OnEvent();
                break;
            }
        }
    }
    public void OnEvent()
    {
        outLineNumber = (int)tutorialDic[currEv]["OutLineNumber"];
        int scriptNumber = (int)tutorialDic[currEv]["ScriptNumber"];
        string currEvText = (string)tutorialDic[currEv]["StringCode"];

        string text = gm.GetStringByTable(currEvText);
        OnTextBox(scriptNumber, text);

        if (outLineNumber != -1)
        {
            OnOutline(outLineNumber);
            outlines[outLineNumber].AddEventOriginalButton(OnNextTutorialEvent);
            outlines[outLineNumber].AddEventOriginalButton(OnEvent);
            //if (currEv != 9999)
            //    outlines[outLineNumber].AddEventOriginalButton(OnEvent);
            //else
            //    return;
        }
        //else
        //{
        //    var iamge = textBoxes[scriptNumber].GetComponent<RawImage>();
        //    TutorialNoneBlockPanels.Instance.SetPanelsSurroundTarget(iamge);
        //}
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

        if (currEv == 9999)
            return;

        Debug.Log("++");
        currEv++;
        if (currEv >= tutorialDic.Count)
        {
            return;
        }
    }

    public void SetOutlineButton(GameObject button, bool needAdjust = true)
    {
        int tempOutLineNumber = (int)tutorialDic[currEv + 1]["OutLineNumber"];

        if (outlines[tempOutLineNumber].originalButton == null)
        {
            outlines[tempOutLineNumber].originalButton = button;
        }

        outlines[tempOutLineNumber].NeedAdjustPos = needAdjust;
    }
}
