using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEffect_77_ObjectMake2 : MonoBehaviour {

    public GameObject m_makeObj;
    public float m_startDelay;
    public float m_destroyTime;
    float m_Time;
    bool isMake = true;

	void Start () {
        m_Time = Time.time;
	}

    void Update() {
        if (isMake == false)
            return;
        if(Time.time > m_Time + m_startDelay && isMake)
        {
            isMake = false;
            GameObject gm = Instantiate(m_makeObj, transform.position, transform.rotation);
            gm.transform.parent = this.transform;
            Destroy(gm, m_destroyTime);
        }
    }
}
