using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogue : MonoBehaviour
{    

    private Dictionary<string, string> tutorialDialouges = new();

    public int tutorialStartIndex = 126;

    private void Start()
    {
        ParseEventTable();
    }

    private void ParseEventTable()
    {
        var eventTable = GameManager.Instance.eventInfoList;

        for(int i = tutorialStartIndex - 2; i < eventTable.Count; i++)
        {
            tutorialDialouges.Add((string)eventTable[i]["ID"], (string)eventTable[i]["Contents"]);
        }

        Logger.Debug(tutorialDialouges["tutorial_string_start1"]);
    }
}
