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

    private void Awake()
    {
        foreach (GameObject go in prefebs)
        {
            go.GetComponent<MissionMarkData>().OnOffMark(false);
        }

        randomNumbers = Utils.DistinctRandomNumbers(7, 4);

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
