using System;
using System.Collections.Generic;
using UnityEngine;

public class ManageHeroWindow : View
{
    public GameObject heroInfoPrefab;
    public Transform contents;
    public List<HeroInfoButton> heroInfos = new ();
    private List<CharacterDataBundle> copyCharacterTable = new ();

    private void Awake()
    {
        SetList();

        int count = GameManager.Instance.playerData.characterCount;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(heroInfoPrefab, contents);
            HeroInfoButton info = obj.GetComponent<HeroInfoButton>();
            heroInfos.Add(info);
        }
    }

    private void SetList()
    {
        copyCharacterTable.Clear();
        var list = GameManager.Instance.myHeroes;
        foreach (var character in list)
        {
            copyCharacterTable.Add(character.GetComponent<CharacterDataBundle>());
        }
    }

    private void OnEnable()
    {
        SetList();
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
        int count = 0;
        foreach (var character in copyCharacterTable)
        {
            heroInfos[count].SetData(character);
            heroInfos[count].gameObject.SetActive(true);
            count++;
        }

        int max = GameManager.Instance.playerData.characterCount;
        for (int index = count; index < max; index++)
        {
            heroInfos[index].gameObject.SetActive(false);
        }
    }
}