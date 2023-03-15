using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csRotate : MonoBehaviour {

    public Vector3 RotateOffset;
    Vector3 RotateMulti;
	
    void Awake()
    {
        RotateMulti = RotateOffset;
    }

	// Update is called once per frame
	void Update ()
    {
        transform.rotation *= Quaternion.Euler(RotateMulti);		
	}
}
