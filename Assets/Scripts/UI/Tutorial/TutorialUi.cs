using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPage
{
    public string id;
    public string title;
    public string image_name;
    public List<string> lines = new();
}

public class TutorialUi : MonoBehaviour
{
    public List<TutorialPage> pages = new();
    public Image image;
    public TextMeshProUGUI text;
    int idx; 
    public List<Dictionary<string, object>> tutorialTable; // 튜토리얼 대사 테이블

    private void Awake()
    {
        idx = 0;
        Init();
    }
    public void Init()
    {
        idx = 0;
        GameManager gm = GameManager.Instance;
        tutorialTable = gm.tutorialTable;

        for (int i = 0; i < tutorialTable.Count; i++)
        {
            var pageData = tutorialTable[i];
            var id = pageData["ID"].ToString();
            var title = pageData["Title"].ToString();
            int count = (int)pageData["Count"];

            TutorialPage page = new TutorialPage();
            page.id = id;
            page.title = title;
            page.image_name = id;

            for (int j = 0; j < count; j++)
            {
                page.lines.Add(gm.GetStringByTable($"{id}_{j}"));
            }
            pages.Add(page);
        }

        SetPage();
    }   

    public void OnLeft()
    {
        idx = (idx + 1 + pages.Count) % pages.Count;
        SetPage();
    }
    public void OnRight()
    {
        idx = (idx - 1 + pages.Count) % pages.Count;
        SetPage();
    }

    public void SetPage()
    {
        text.text = string.Empty;
        image.sprite = GameManager.Instance.GetSpriteByAddress(pages[idx].image_name);
        text.text = string.Join("\n", pages[idx].lines);
    }

    public void OnClickExit()
    {
        UIManager.Instance.ClearPopups();
    }
}
