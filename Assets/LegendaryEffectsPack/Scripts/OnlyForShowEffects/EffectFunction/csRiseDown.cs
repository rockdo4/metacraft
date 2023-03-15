using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csRiseDown : MonoBehaviour {

    public float m_y_Value;
    public float m_durationTime;
    public float m_endTime;
    public float m_lerpTime; 
    float m_max_Y_Coord;
    float m_Time;
    Vector3 m_original_Pos;
    public float m_startTime;

    void Start()
    {
        m_original_Pos = transform.position;
    }

    void Update()
    {
        m_Time += Time.deltaTime;

        if (m_Time > m_startTime)
        {
            if (m_Time < m_durationTime + m_startTime)
                transform.position = Vector3.Lerp(this.transform.position, m_original_Pos + new Vector3(0, m_y_Value, 0), Time.deltaTime * m_lerpTime);
            if (m_Time > m_endTime + m_startTime)
                transform.position = Vector3.Lerp(this.transform.position, m_original_Pos - new Vector3(0, 1, 0), Time.deltaTime * m_lerpTime * 0.5f);
        }
    }
}