using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionSpawner : MonoBehaviour
{
    public GameObject[] prefebs;

    private List<int> randomNumbers;

    private void Awake()
    {
        foreach (GameObject go in prefebs)
        {
            go.GetComponent<MissionMarkData>().OnOffMark(false);
        }

        if(GameManager.Instance.playerData.isTutorial)
        {
            CreateMission(1);
            return;
        }
        randomNumbers = Utils.DistinctRandomNumbers(7, 3);

        for (int i = 0; i < randomNumbers.Count; i++)
        {
            CreateMission(randomNumbers[i]);
        }
    }

    private void CreateMission(int index)
    {
        prefebs[index].GetComponent<MissionMarkData>().OnOffMark(true);
    }
}