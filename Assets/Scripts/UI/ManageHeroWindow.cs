using System;
using System.Collections.Generic;
using UnityEngine;

public class ManageHeroWindow : View
{
    public GameObject heroInfoPrefab;
    public Transform contents;
    public List<HeroInfo> heroInfos = new ();

    public void SelectSortType(Int32 value)
    {
        Logger.Debug($"{value}");
    }

    private void Start()
    {
        var characters = GameManager.Instance.characters;
        Logger.Debug(characters.Count);
        foreach (var character in characters)
        {
            GameObject obj = Instantiate(heroInfoPrefab, contents);
            HeroInfo info = obj.GetComponent<HeroInfo>();
            info.SetData(character);
            heroInfos.Add(info);
        }
    }
}