using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEffect_03_ArrowFunction : MonoBehaviour {

    public float m_durationTime;
    public float m_lerpValue;
    float m_Time;
    Vector3 m_originalPos;
    Vector3 m_movePos;
    public float m_objectDestroyTime;
    public Vector3 m_origianlPosSet;
    public GameObject m_makeObj;
	
    float RandomValue(float x)
    {
        float rnd = Random.Range(-x, x);
        return rnd;
    }

	void Start ()
    {
        m_Time = Time.time;
        m_movePos = transform.position;
        transform.position =  new Vector3(
            RandomValue(m_origianlPosSet.x),
            m_origianlPosSet.y, RandomValue(m_origianlPosSet.z)
            );
        m_originalPos = transform.position;
        transform.rotation = Quaternion.LookRotation(transform.position - m_movePos);
    }

	void Update ()
    {
        transform.position = Vector3.Lerp(this.transform.position,m_movePos, Time.deltaTime * m_lerpValue);

        if(Time.time > m_Time + m_durationTime)
        {
            Destroy(gameObject);
            GameObject m_obj = Instantiate(m_makeObj, transform.position, Quaternion.identity);
            Destroy(m_obj, m_objectDestroyTime);
        }
    }
}
