using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageHeroWindow : View
{
    public GameObject heroInfoPrefab;
    public Transform contents;
    public List<HeroInfoButton> heroInfos = new ();
    private List<CharacterDataBundle> copyCharacterDatas;
    public Scrollbar scrollBar;

    private void Awake()
    {
        copyCharacterDatas = GameManager.Instance.characterTable;

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
                copyCharacterDatas.Sort((x, y) => { return x.data.grade.CompareTo(y.data.grade); });
                break;

            case 2:
                copyCharacterDatas.Sort((x, y) => { return x.data.job.CompareTo(y.data.job); });
                break;

            case 3:
                copyCharacterDatas.Sort((x, y) => { return x.data.level.CompareTo(y.data.level); });
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