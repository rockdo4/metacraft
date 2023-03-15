using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObjectMoveDestroy2 : MonoBehaviour {

    public GameObject m_gameObjectMain;
    public GameObject m_gameObjectTail;
    GameObject m_makedObject;
    public float DestroyTime;
    public float maxTime = 1;
    float time;
    public float MoveSpeed = 10;

    void LateUpdate()
    {
        time += Time.deltaTime;
        if (time > maxTime)
        {
            m_gameObjectTail.transform.parent = null;
            Destroy(this.gameObject);
            Destroy(m_gameObjectTail, DestroyTime);
        }
        transform.Translate(Vector3.forward * -Time.deltaTime * MoveSpeed);
    }
}
