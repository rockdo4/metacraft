using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEffect_79_WallRiseDown : MonoBehaviour {

    public float m_durationTime;
    public float m_y_Pos;
    public float m_lerpTime;
    float m_Time;
    Vector3 m_originalPos;

    void Start() {
        m_Time = Time.time;
        m_originalPos = transform.position;
    }

	void Update () {
		if(Time.time < m_Time + m_durationTime)
            transform.position = Vector3.Lerp(transform.position, new Vector3(m_originalPos.x, m_y_Pos, m_originalPos.z), Time.deltaTime * m_lerpTime);
        else
            transform.position = Vector3.Lerp(transform.position, new Vector3(m_originalPos.x, m_originalPos.y, m_originalPos.z), Time.deltaTime * m_lerpTime);
    }
}
