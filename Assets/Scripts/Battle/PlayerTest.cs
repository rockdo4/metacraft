using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTest : MonoBehaviour
{
    public GameObject movePoint;

    private void Start()
    {
        var nav = gameObject.GetComponent<NavMeshAgent>();
        nav.SetDestination(movePoint.transform.position);
    }
}
