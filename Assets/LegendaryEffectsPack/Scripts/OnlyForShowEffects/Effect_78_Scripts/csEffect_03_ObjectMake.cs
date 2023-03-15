using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEffect_03_ObjectMake : MonoBehaviour {

    public float m_startDelay;
    public GameObject[] m_makeObjs;
    public float m_makeDelay;
    public int m_makeCount;
    public float m_destroyTime;
    public Vector3 m_vectorRandom;
    public Transform m_makePos;
    float m_Time;
    float m_Time2;
    int m_count;

    float RandomValue(float x)
    {
        float rnd = Random.Range(-x, x);
        return rnd;
    }

    void Start ()
    {
        m_Time = m_Time2 = Time.time;
	}
	
	void Update ()
    {
        if (Time.time < m_Time + m_startDelay)
            return;
        if(Time.time > m_Time2 + m_makeDelay && m_count < m_makeCount)
        {
            m_Time2 = Time.time;
            float m_posX = RandomValue(m_vectorRandom.x);
            float m_posY = RandomValue(m_vectorRandom.y);
            float m_posZ = RandomValue(m_vectorRandom.z);

            for (int i = 0; i < m_makeObjs.Length; i++)
            {
                GameObject m_obj = Instantiate(m_makeObjs[i], m_makePos.position +
                    new Vector3(m_posX, m_posY,m_posZ),Quaternion.identity);
                m_obj.transform.parent = this.transform;
                Destroy(m_obj, m_destroyTime);
            }
            m_count++;
        }
	}
}
