using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogue : MonoBehaviour
{
    private List<List<string>> tutorialDialouges;

    private string keyHead = "tutorial_string_";
    public string[] keyTail;

    private void Start()
    {
        ParseEventTable();
    }

    private void ParseEventTable()
    {
        GameManager gm = GameManager.Instance;
        
        tutorialDialouges = new(keyTail.Length);

        for(int i = 0; i < tutorialDialouges.Capacity; i++)
        {
            tutorialDialouges.Add(new (10));
            int dialougeNum = 1;
            while(dialougeNum < 100)
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
}
