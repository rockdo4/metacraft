using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageHeroWindow : View
{
    public GameObject heroInfoPrefab;
    public Transform contents;
    public List<HeroInfoButton> heroInfos = new ();
    private List<CharacterDataBundle> copyCharacterTable;
    public Scrollbar scrollBar;

    private void Awake()
    {
        copyCharacterTable = GameManager.Instance.characterTable;

        int count = copyCharacterTable.Count;
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
                copyCharacterTable.Sort();
                break;

            case 1:
                copyCharacterTable.Sort((x, y) => { return x.data.grade.CompareTo(y.data.grade); });
                break;

            case 2:
                copyCharacterTable.Sort((x, y) => { return x.data.job.CompareTo(y.data.job); });
                break;

            case 3:
                copyCharacterTable.Sort((x, y) => { return x.data.level.CompareTo(y.data.level); });
                break;
        }

        SetInfos();
    }

    private void SetInfos()
    {
        int index = 0;
        foreach (var character in copyCharacterTable)
        {
            heroInfos[index].SetData(character);
            index++;
        }
    }
}