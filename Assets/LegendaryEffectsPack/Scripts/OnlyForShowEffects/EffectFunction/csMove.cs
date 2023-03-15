using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMove : MonoBehaviour {

    public float MoveSpeed;
    float m_Time;

	void Update ()
    {
        m_Time += Time.deltaTime;
        transform.Translate(Vector3.forward * -Time.deltaTime * MoveSpeed);
    }
}
