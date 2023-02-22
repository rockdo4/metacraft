using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class MissionSpawner : MonoBehaviour
{
    public Transform[] points;
    public GameObject prefeb;

    private List<int> randomNumbers;

    private void Start()
    {
        randomNumbers = MakeRandomNumbers(7, 4);

        for (int i = 0; i < randomNumbers.Count; i++)
        {
            CreateMission(points[randomNumbers[i]]);
        }
    }

    private List<int> MakeRandomNumbers(int totalNumber, int requiredNumber)
    {
        List<int> values = new List<int>();
        List<int> spwanIndexes = new List<int>();
        for (int i = 0; i < totalNumber; i++)
        {
            values.Add(i);
        }

        for (int i = 0; i < requiredNumber; ++i)
        {
            int a = Random.Range(0, values.Count);
            spwanIndexes.Add(values[a]);
            values.RemoveAt(a);
        }

        return spwanIndexes;
    }

    private void CreateMission(Transform point)
    {
        Instantiate(prefeb, point);
    }
}
