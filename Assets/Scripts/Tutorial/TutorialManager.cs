using System.Collections.Generic;
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
    }

    public void OnClickSkip()
    {
        for (int i = currEv; i < tutorialDic.Count; i++)
        {
            bool skipSection = (bool)tutorialDic[i]["IsSkipStop"];
            if (skipSection)
            {
                currEv = i;
                OnEvent();
                break;
            }
        }
    }
    public void OnEvent()
    {
        int outlineNumber = (int)tutorialDic[currEv]["OutLineNumber"];
        int scriptNumber = (int)tutorialDic[currEv]["ScriptNumber"];
        string currEvText = (string)tutorialDic[currEv]["StringCode"];

        OnCurrOutline(outlineNumber);
        string text = gm.GetStringByTable(currEvText);
        OnCurrTextBox(scriptNumber, text);
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
