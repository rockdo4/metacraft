using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageHeroWindow : View
{
    public GameObject heroInfoPrefab;
    public Transform contents;
    public List<HeroInfoButton> heroInfos = new ();
    private List<HeroData> copyCharacterDatas;
    public Scrollbar scrollBar;

    private void Awake()
    {
        copyCharacterDatas = GameManager.Instance.newCharacters;

        int count = copyCharacterDatas.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(heroInfoPrefab, contents);
            HeroInfoButton info = obj.GetComponent<HeroInfoButton>();
            heroInfos.Add(info);
        }
    }

    private void OnEnable()
    {
        SelectSortType(0);
    }

    public void SelectSortType(Int32 value)
    {
        switch (value)
        {
            case 0:
                copyCharacterDatas.Sort();
                break;

            case 1:
                copyCharacterDatas.Sort((x, y) => { return x.info.grade.CompareTo(y.info.grade); });
                break;

            case 2:
                copyCharacterDatas.Sort((x, y) => { return x.info.job.CompareTo(y.info.job); });
                break;

            case 3:
                copyCharacterDatas.Sort((x, y) => { return x.info.level.CompareTo(y.info.level); });
                break;
        }

        SetInfos();
    }

    private void SetInfos()
    {
        int index = 0;
        foreach (var character in copyCharacterDatas)
        {
            heroInfos[index].SetData(character);
            index++;
        }
    }
}