using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEffect_77_ObjectMake : MonoBehaviour {

    public GameObject m_makeObj;
    public float m_makeDelay;
    public float m_makeDuration;
    public float m_destroyTime = 1.0f;
    public Vector3 m_addedPos;
    float m_Time;
    float m_Time2;

	void Start () {
        m_Time = m_Time2 = Time.time;
	}
	
	void Update () {
		if(Time.time < m_Time + m_makeDuration)
        {
            if(Time.time > m_Time2 + m_makeDelay)
            {
                GameObject m_obj = Instantiate(m_makeObj, transform.position +
                    new Vector3(Random.Range(m_addedPos.x, -m_addedPos.x),
                    Random.Range(m_addedPos.y, -m_addedPos.y),
                    Random.Range(m_addedPos.z, -m_addedPos.z)),transform.rotation);
                m_obj.transform.parent = this.transform;
                m_Time2 = Time.time;
                Destroy(m_obj, m_destroyTime);
            }
        }
	}
}
