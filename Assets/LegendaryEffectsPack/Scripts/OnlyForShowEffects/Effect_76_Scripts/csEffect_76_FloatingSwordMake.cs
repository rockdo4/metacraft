using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEffect_76_FloatingSwordMake : MonoBehaviour {

    public Transform m_makePoint;
    public Transform m_movePoint;
    public GameObject m_gameObject;
    public float m_object_MakeDelay;
    public float m_DestroyTime;
    public Vector3 m_randomValue;
    public int m_object_MakeCount;
    float m_Time;
    float m_count;

    void Start()
    {
        m_Time = Time.time;
    }

	void Update () {
        if(Time.time >m_Time + m_object_MakeDelay && m_count < m_object_MakeCount)
        {
            Vector3 m_addedPos = new Vector3(
                Random.Range(-m_randomValue.x,m_randomValue.x),
                Random.Range(-m_randomValue.y, m_randomValue.y),
                Random.Range(-m_randomValue.z, m_randomValue.z));
            GameObject ob = Instantiate(m_gameObject, m_makePoint.position+ m_addedPos, Quaternion.identity);

            csEffect_76_Animation m_effectScripts = 
                ob.GetComponent< csEffect_76_Animation >(); //Get Effect_01_Animation scripts from sword object.

            m_effectScripts.m_secondMovePos.x = m_movePoint.position.x; //Set x position to secondMove vector variable
            m_effectScripts.m_secondMovePos.y = m_movePoint.position.y; //Set y position to secondMove vector variable
            m_effectScripts.m_secondMovePos.z = m_movePoint.position.z; //Set z position to secondMove vector variable
            ob.transform.parent = this.transform;
            Destroy(ob, m_DestroyTime);
            m_Time = Time.time;
            m_count++;
        }
    }
}
