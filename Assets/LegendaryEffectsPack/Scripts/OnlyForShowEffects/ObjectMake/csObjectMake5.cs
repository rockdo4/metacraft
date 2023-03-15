using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObjectMake5 : MonoBehaviour {

    public GameObject m_gameObject;
    public float m_objectSize;
    public float m_subtractYValue;
    public float m_object_MakeCount;
    public float m_object_MakeDelay;
    public bool m_isCrossMake;
    public float m_destroyTime;
    float m_Time;
    float m_count;

    void Update()
    {
        m_Time += Time.deltaTime;
        Vector3 addedPos = new Vector3(0,0,0);
        int crossMake = 0;

        if (m_Time > m_object_MakeDelay && m_count < m_object_MakeCount)
        {
            if (m_isCrossMake)
            {
                if (m_count % 2 == 0)
                    crossMake = 1;
                else
                    crossMake = -1;
            }

            addedPos = transform.forward * m_objectSize * m_count;
            Vector3 pos = transform.position - new Vector3(0, m_subtractYValue, 0) + addedPos + (transform.right  * crossMake);
            Quaternion rot = transform.rotation;

            GameObject ob = Instantiate(m_gameObject, pos, rot);
            ob.transform.parent = this.transform;
            Destroy(ob, m_destroyTime);

            m_Time = 0;
            m_count++;
        }
    }
}
