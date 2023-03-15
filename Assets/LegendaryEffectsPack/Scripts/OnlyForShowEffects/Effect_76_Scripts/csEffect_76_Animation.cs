using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEffect_76_Animation : MonoBehaviour {

    //--First move ( when sword object down ) variables--
    public float m_y_StopPos;
    public float m_y_randomValue;
    public float m_durationTime;
    public float m_firstLerpValue;
    //--------------------------------------------------------
    //--Second look & move variables--
    public float m_lookAtTime; //Second look at time
    public bool m_onlyYValueMove; // check this when animate like first move ( just down/up using y value )
    public float m_secondMoveTime;
    [HideInInspector]
    public Vector3 m_secondMovePos; //This is second move & look at position. 
    public float m_secondLerpValue;
    public GameObject m_particleObject;
    public float m_particleDestroyTime = 1.0f;
    public float m_downYPos;
    //----------------------------------
    float m_addedRandom;
    Vector3 m_originalPos;
    float m_Time;
    bool isMake = true;

    void Start()
    {
        m_Time = Time.time;
        m_originalPos = transform.position;
        if(m_y_randomValue != 0)
        m_addedRandom = Random.Range(m_y_randomValue, 0);
    }

    void FirstMove()
    {
        if (Time.time < m_Time + m_durationTime)
            transform.position = Vector3.Lerp(this.transform.position, 
                new Vector3(m_originalPos.x, m_y_StopPos + m_addedRandom, m_originalPos.z),
                Time.deltaTime * m_firstLerpValue);
    }

    void LookAtPoint()
    {
        if (m_lookAtTime != 0)
        {
            if (Time.time < m_Time + m_lookAtTime)
            {
                Quaternion lookPos = Quaternion.LookRotation(transform.position - m_secondMovePos);
                transform.rotation = Quaternion.Slerp(transform.rotation, (lookPos), Time.deltaTime * m_firstLerpValue);
            }
        }
    }

    void SecondMove()
    {
        if (m_secondMoveTime != 0)
        {
            if (Time.time > m_Time + m_secondMoveTime)
            {
                //Check when make particle. if m_particleObject is empty, this is not work.
                if (isMake && m_particleObject)
                {
                    isMake = false;
                    GameObject m_obj = Instantiate(m_particleObject, transform.position, transform.rotation);
                    Destroy(m_obj, 1f);
                }
                //Check move like FirstMove ( Only move up and down )
                if (m_onlyYValueMove)
                {
                    Vector3 m_originalPos = transform.position;
                    transform.position = Vector3.Lerp(transform.position, m_originalPos + new Vector3(0, m_secondMovePos.y+m_downYPos, 0), Time.deltaTime * m_secondLerpValue);
                }
                else
                    transform.position = Vector3.Lerp(transform.position, m_secondMovePos, Time.deltaTime * m_secondLerpValue);
                Destroy(gameObject, m_particleDestroyTime);
            }
        }
    }

    void Update()
    {
        FirstMove();
        LookAtPoint();
        SecondMove();
    }
}