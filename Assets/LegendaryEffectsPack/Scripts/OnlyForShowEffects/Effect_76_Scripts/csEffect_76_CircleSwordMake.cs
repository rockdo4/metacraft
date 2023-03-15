using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEffect_76_CircleSwordMake : MonoBehaviour {

    public Transform m_makePoint;
    public GameObject m_gameObject;
    public float m_startDelay;
    public float m_object_MakeDelay;
    public float m_DestroyTime;
    public float m_interval;
    public float m_yAddedPos = 0.0f;
    public int m_object_MakeCount;
    
    float m_Time;
    float m_Time_startDelay;
    float m_count;

    void Start()
    {
        m_Time = Time.time;
        m_Time_startDelay = Time.time;
    }

	void Update ()
    {
        if (Time.time < m_Time_startDelay + m_startDelay)
            return;

        if (Time.time >m_Time + m_object_MakeDelay && m_count < m_object_MakeCount)
        {
            float Angle = 2.0f * Mathf.PI / m_object_MakeCount* m_count;
            float pos_X = Mathf.Cos(Angle) * m_interval;
            float pos_Z = Mathf.Sin(Angle) * m_interval;

            GameObject ob = Instantiate(m_gameObject, m_makePoint.position + new Vector3(pos_X, m_yAddedPos, pos_Z), m_gameObject.transform.rotation);
            ob.transform.parent = this.transform;
            Destroy(ob, m_DestroyTime);
            m_Time = Time.time;
            m_count++;
        }
    }
}
