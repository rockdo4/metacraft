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

        if (outLineNumber != -1)
        {
            OnOutline(outLineNumber);
        }
        //else
        //{
        //    outlines[currEv - 1].SetActive(false);
        //}

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
        for (int i = 0; i < textBoxes.Count; i++)
        {
            outlines[i].SetActive(false);
        }

        outlines[index].SetActive(true);
    }

    public void OnNextTutorialEvent()
    {
        outlines[currEv].SetActive(false);
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
