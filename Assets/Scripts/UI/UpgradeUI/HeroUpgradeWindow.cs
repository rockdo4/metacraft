using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroUpgradeWindow : View
{
    public GameObject heroInfoPrefab;
    public Transform contents;
    public List<HeroInfoButton> heroInfos = new ();
    private List<CharacterDataBundle> copyCharacterTable = new ();

    private int value;

    private void Awake()
    {
        SetList();

        int count = GameManager.Instance.heroDatabase.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(heroInfoPrefab, contents);
            HeroInfoButton info = obj.GetComponent<HeroInfoButton>();

            if ((CharacterGrade)info.data.grade != CharacterGrade.SS)
            {
                heroInfos.Add(info);
            }
        }
    }

    private void SetList()
    {
        copyCharacterTable.Clear();
        var list = GameManager.Instance.myHeroes;
        foreach (var character in list)
        {
            copyCharacterTable.Add(character.Value.GetComponent<CharacterDataBundle>());
        }

        int count = copyCharacterTable.Count - 1;
        for (int i = count; i >= 0; i--)
        {
            if ((CharacterGrade)copyCharacterTable[i].data.grade == CharacterGrade.SS)
            {
                copyCharacterTable.Remove(copyCharacterTable[i]);
            }
        }
    }

    private void OnEnable()
    {
        SetList();
        SelectSortType(value);
    }

    public void SelectSortType(Int32 value)
    {
        this.value = value;

        switch (value)
        {
            case 0:
                copyCharacterTable.Sort();
                break;

            case 1:
                copyCharacterTable.Sort((x, y) => { return -1 * x.data.grade.CompareTo(y.data.grade); });
                break;

            case 2:
                copyCharacterTable.Sort((x, y) => { return x.data.job.CompareTo(y.data.job); });
                break;

            case 3:
                copyCharacterTable.Sort((x, y) => { return -1 * x.data.level.CompareTo(y.data.level); });
                break;
        }

        SetInfos();
    }

    private void SetInfos()
    {
        int count = 0;
        foreach (var character in copyCharacterTable)
        {
            if ((CharacterGrade)heroInfos[count].data.grade == CharacterGrade.SS)
            {
                heroInfos[count].gameObject.SetActive(false);
                heroInfos.Remove(heroInfos[count]);
                count++;
                continue;
            }

            heroInfos[count].SetData(character);
            heroInfos[count].gameObject.SetActive(true);
            count++;
        }

        //int max = GameManager.Instance.heroDatabase.Count;
        int max = heroInfos.Count;
        for (int index = count; index < max; index++)
        {
            heroInfos[index].gameObject.SetActive(false);
        }
    }
}