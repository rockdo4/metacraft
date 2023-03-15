using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csLookAt : MonoBehaviour
{
    public Transform ts;
	
	void Update () {
        transform.LookAt(ts);
	}
}
