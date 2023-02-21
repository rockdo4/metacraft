using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSpawner : MonoBehaviour
{
    public Transform[] points;
    public GameObject prefeb;

    private Transform[] slectedPoints;

    private void Start()
    {
        
    }

    private void RandomPoints(int count)
    {

        for (int i = 0; i < count; i++)
        {
            //while(true)
            //{
            //    //slectedPoints[i] = 
            //}
        }
        Random.Range(0,points.Length-1);
        
    }
}
