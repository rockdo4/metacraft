using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csParticleStop : MonoBehaviour {

    public ParticleSystem[] m_particleMembers;
    public float m_stopTime;
    float m_Time;

	void Start () {
        m_Time = Time.time;
	}
	

	void Update () {
		if(Time.time > m_Time + m_stopTime)
        {
            for (int i = 0; i < m_particleMembers.Length; i++)
                if(m_particleMembers[i])
                     m_particleMembers[i].Stop();
        }
	}
}
