using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class MissionSpawner : MonoBehaviour
{
    public GameObject[] prefebs;

    private List<int> randomNumbers;

    //private void Awake()
    //{
    //    foreach (GameObject go in prefebs)
    //    {
    //        go.GetComponent<MissionMarkData>().OnOffMark(false);
    //    }

    //    randomNumbers = Utils.DistinctRandomNumbers(7, 4);

    //    for (int i = 0; i < randomNumbers.Count; i++)
    //    {
    //        prefebs[i].GetComponent<MissionMarkData>().OnOffMark(true);
    //    }
    //}

    private GameObject[] CreateMission(int count)
    {
        foreach (GameObject go in prefebs)
        {
            go.GetComponent<MissionMarkData>().OnOffMark(false);
        }

        randomNumbers = Utils.DistinctRandomNumbers(7, count);

        for (int i = 0; i < randomNumbers.Count; i++)
        {
            prefebs[i].GetComponent<MissionMarkData>().OnOffMark(true);
        }

        return prefebs;
    }
}
