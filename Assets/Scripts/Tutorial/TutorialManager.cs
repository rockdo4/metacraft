using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        if (!gm.playerData.isTutorial)
        {
            Destroy(gameObject);
        }
        else
        {
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
        int outlineNumber = (int)tutorialDic[currEv]["OutLineNumber"];
        int scriptNumber = (int)tutorialDic[currEv]["ScriptNumber"];
        string currEvText = (string)tutorialDic[currEv]["StringCode"];

        string text = gm.GetStringByTable(currEvText);
        OnTextBox(scriptNumber, text);
        OnOutline(outlineNumber);
    }

    private void OnTextBox(int index, string text)
    {
        for (int i = 0; i < textBoxes.Count; i++)
        {
            textBoxes[i].gameObject.SetActive(false);
        }

        textBoxes[currEv].gameObject.SetActive(true);
        textBoxes[currEv].textBox.text = text;
    }

    private void OnOutline(int index)
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

        //GameObject asd = new();
        //var trigger = asd.GetComponent<EventTrigger>();

        //EventTrigger.Entry entry = new() { eventID = EventTriggerType.PointerUp };
        //entry.callback.AddListener((eventData) => { OnEvent(); });
        //trigger.triggers.Add(entry);
    }
}
