using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csObjectMake : MonoBehaviour {

    public GameObject m_gameObject;
    float t;
    public float Delay = 0.0f;
    public float MaxRage;

    void Update()
    {
        t += Time.deltaTime;
        if (Delay < t)
        {
            t = 0.0f;
            GameObject gm = Instantiate(m_gameObject, transform.position + new Vector3(Random.Range(-MaxRage, MaxRage), Random.Range(-MaxRage, MaxRage), Random.Range(-MaxRage, MaxRage)), transform.rotation);
            gm.transform.parent = this.transform;
            Destroy(gm, 5f);
        }
    }
}
