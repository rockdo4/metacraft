using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEffect_80_SunMove : MonoBehaviour {

    public Transform m_movePos;
    public float m_lerpValue;
    public float DestroyTime;

    void Start() {
        Destroy(gameObject, DestroyTime);
    }

	void Update () {
        transform.position = Vector3.Lerp(transform.position, m_movePos.position, Time.deltaTime * m_lerpValue);
	}
}
