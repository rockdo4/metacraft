using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObjectMoveDestroy : MonoBehaviour {

    public GameObject m_gameObjectMain;
    public GameObject m_gameObjectTail;
    GameObject m_makedObject;
    public Transform m_hitObject;
    public float maxLength;
    public float DestroyTime;
    public float DestroyTime2 = 2;
    public float maxTime = 1;
    float time;
    bool ishit;
    public float MoveSpeed = 10;

    void LateUpdate()
    {
      //  time += Time.deltaTime;
       // if (time > maxTime)
       //     Destroy(this.gameObject);
        
        transform.Translate(Vector3.forward * -Time.deltaTime * MoveSpeed);
        if (!ishit)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.forward, out hit, maxLength))
                HitObj(hit);
        }
    }

    void HitObj(RaycastHit hit)
    {
        ishit = true;
        m_gameObjectTail.transform.parent = null;
        m_makedObject = Instantiate(m_hitObject, hit.point, Quaternion.LookRotation(hit.normal)).gameObject;
        Destroy(this.gameObject);
        Destroy(m_gameObjectTail, DestroyTime);
        Destroy(m_makedObject, DestroyTime2);
    }
}
