using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csScaleChange : MonoBehaviour
{
    float m_ScaleFactor = 1;
    public float m_Weight;
    public bool isScaleUp;
    public float m_startTime;
    float m_Time;

	void Update () {
        m_Time += Time.deltaTime;
        if (m_Time < m_startTime)
            return;

        m_Weight += Time.deltaTime/20;
        transform.localScale *= m_ScaleFactor;

        if(!isScaleUp)
           m_ScaleFactor = m_ScaleFactor - (Time.deltaTime * m_Weight * 0.01f);
        else
           m_ScaleFactor = m_ScaleFactor + (Time.deltaTime * m_Weight * 0.01f);

        if (m_ScaleFactor < 0)
            m_ScaleFactor = 0;
	}
}
