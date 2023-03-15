using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObjectMake2 : MonoBehaviour {

    public GameObject m_gameObject;
    public float m_object_MakeDelay;
    public float m_object_MakeRange;
    public int m_object_MakeCount;
    public float m_startDelay;
    float m_Time;
    float m_object_MakeRange2;
    float m_count;
    public bool isX;
    public bool isY;
    public bool isZ;
    public bool isMinusZ;

	void Update () {
        m_Time += Time.deltaTime;

        if(m_startDelay > 0)
        {
            if (m_Time > m_startDelay)
                m_startDelay = 0;
            else
                return;
        }

        if(m_Time > m_object_MakeDelay && m_count < m_object_MakeCount)
        {
            Vector3 addedPosition = new Vector3(0,0,0);
            if (isX)
                addedPosition += new Vector3(m_object_MakeRange2, 0, 0);
            if(isY)
                addedPosition += new Vector3(0, m_object_MakeRange2, 0);
            if (isZ)
            {
                if (isMinusZ)
                    m_object_MakeRange2 *= -1;
                addedPosition += new Vector3(0, 0, m_object_MakeRange2);
                if (isMinusZ)
                    m_object_MakeRange2 *= -1;
            }

            m_Time = 0;
           GameObject ob = Instantiate(m_gameObject, transform.position+addedPosition, transform.rotation);
            ob.transform.parent = this.transform;
            Destroy(ob, 6f);
            m_object_MakeRange2 += m_object_MakeRange;
            m_count++;
        }

	}
}
