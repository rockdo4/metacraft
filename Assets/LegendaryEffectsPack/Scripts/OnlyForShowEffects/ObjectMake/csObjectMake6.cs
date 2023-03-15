using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObjectMake6 : MonoBehaviour {

    public GameObject m_gameObject;
    public float m_object_MakeDelay;
    public float m_DestroyTime;
    public float m_interval;
    public float m_yPos = 0.0f;
    public int m_object_MakeCount;
    float m_Time;
    float m_count;

	void Update () {
        m_Time += Time.deltaTime;
		
        if(m_Time > m_object_MakeDelay && m_count < m_object_MakeCount)
        {
            float Angle = 2.0f * Mathf.PI / m_object_MakeCount* m_count;
            float pos_X = Mathf.Cos(Angle) * m_interval;
            float pos_Z = Mathf.Sin(Angle) * m_interval;

            m_Time = 0.0f;
            GameObject ob = Instantiate(m_gameObject, transform.position + new Vector3(pos_X, m_yPos, pos_Z), Quaternion.LookRotation(new Vector3(pos_X,0,pos_Z)));
            ob.transform.parent =this.transform;
            Destroy(ob, m_DestroyTime);
            m_count++;
        }
    }
}
