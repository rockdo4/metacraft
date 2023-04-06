using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private GameManager gm;
    private List<Dictionary<string, object>> tutorialDic; 
    public List<TutorialEvent> textBoxes;
    public List<GameObject> outlines;

    private static int currEv = 0;

    private void Start()
    {
        gm = GameManager.Instance;
        tutorialDic = gm.tutorialIndexTable;
        if(gm.playerData.isTutorial)
        {
            OnEvent();
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            OnNextTutorialEvent();
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
        int outLineNumber = (int)tutorialDic[currEv]["OutLineNumber"];
        int scriptNumber = (int)tutorialDic[currEv]["ScriptNumber"];
        string currEvText = (string)tutorialDic[currEv]["StringCode"];

        if(outLineNumber!=-1)
        {
            OnCurrOutline(outLineNumber);
        }
        else
        {
           outlines[currEv-1].SetActive(false);
        }
        string text = gm.GetStringByTable(currEvText);
        OnCurrTextBox(scriptNumber,text);
    }

    private void OnCurrTextBox(int index, string text)
    {
        for (int i = 0; i < textBoxes.Count; i++)
        {
            textBoxes[i].gameObject.SetActive(false);
        }

        textBoxes[index].gameObject.SetActive(true);
        textBoxes[index].textBox.text = text;
    }

    private void OnCurrOutline(int index)
    {
        for (int i = 0; i < textBoxes.Count; i++)
        {
            outlines[i].SetActive(false);
        }

        outlines[index].SetActive(true);
        //outlines[index].rectTr.position = outlines[index].button.rect.center;
    }

    public void OnNextTutorialEvent()
    {
        //bool scriptClick = (bool)tutorialDic[currEv]["ScriptClick"];
        //bool outlineClick = (bool)tutorialDic[currEv]["OutlineClick"];

        //if (scriptClick)
        //{

        //}
        //else if (outlineClick)
        //{

        //}

        currEv++;
        if (currEv >= tutorialDic.Count)
        {
            return;
        }

        OnEvent();
    }
}
