using System;
using System.Collections.Generic;
using UnityEngine;

public class ManageHeroWindow : View
{
    public GameObject heroInfoPrefab;
    public Transform contents;
    public List<HeroInfo> heroInfos = new ();
    private List<CharacterData> copyCharacterDatas;

    public void SelectSortType(Int32 value)
    {
        switch (value)
        {
            case 0:
                copyCharacterDatas.Sort();
                break;

            case 1:
                copyCharacterDatas.Sort((x, y) => { return x.grade.CompareTo(y.grade); });
                break;

            case 2:
                copyCharacterDatas.Sort((x, y) => { return x.type.CompareTo(y.type); });
                break;

            case 3:
                copyCharacterDatas.Sort((x, y) => { return x.level.CompareTo(y.level); });
                break;
        }

        SetInfos();
    }

    private void Start()
    {
        // heroInfos Initialize
        copyCharacterDatas = GameManager.Instance.characters;
        int count = copyCharacterDatas.Count;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(heroInfoPrefab, contents);
            HeroInfo info = obj.GetComponent<HeroInfo>();
            heroInfos.Add(info);
        }

        SetInfos();
    }

    private void OnEnable()
    {
        copyCharacterDatas = GameManager.Instance.characters;
        if (heroInfos.Count != 0)
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