using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObjectMake3 : MonoBehaviour {

    public GameObject m_gameObject;
    public Transform[] m_makePositions;
    public float m_makeDelay;
    public float m_destroyTime;
    public float m_durationTime;
    float m_Time;
    float m_Time2;



	void Update () {
        m_Time += Time.deltaTime;
        m_Time2 += Time.deltaTime;

        if (m_Time2 > m_durationTime)
            return;

        if(m_Time > m_makeDelay)
        {
            m_Time = 0;
            int a = Random.Range(0, m_makePositions.Length);
            GameObject ob = Instantiate(m_gameObject, m_makePositions[a].position, m_makePositions[a].rotation);
            ob.transform.parent = this.transform;
            Destroy(ob, m_destroyTime);
        }

	}
}
