using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObjectMake4 : MonoBehaviour {

    public GameObject m_gameObject;
    float t = 2;
    int count;
    public float MaxRage;
    public float MaxTime;
    public float MaxCount;
    public int MakeCount;

    void Update()
    {
        if (count > MaxCount)
            return;

        t += Time.deltaTime;
        if(MaxTime < t)
        {
            count++;
            t = 0;
            for (int i = 0; i < MakeCount; i++)
            {
                GameObject ob = Instantiate(m_gameObject, transform.position + new Vector3(Random.Range(-MaxRage, MaxRage), 20 + Random.Range(-MaxRage / 5, MaxRage / 5), Random.Range(-MaxRage, MaxRage)), m_gameObject.transform.rotation);
                ob.transform.parent = this.transform;
                Destroy(ob, 5f);
            }
        }
    }
}
