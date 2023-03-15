using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMaterial_Change : MonoBehaviour
{
    public Material m_inputMaterial;
    Material m_objectMaterial;
    MeshRenderer m_meshRenderer;
    public float m_timeToReduce;
    public float m_reduceFactor =1.0f;
    public float m_timeToStart;
    public float m_startFactor =1.0f;
    float m_time;
    float m_cutOutFactor;

    void Awake()
    {
        m_meshRenderer = gameObject.GetComponent<MeshRenderer>();
        m_meshRenderer.material = m_inputMaterial;
        m_objectMaterial = m_meshRenderer.material;
        if (m_timeToStart > 0)
            m_cutOutFactor = 1.0f;
        else
            m_cutOutFactor = 0.0f;
    }

	void LateUpdate()
    {
        if (m_timeToReduce <= 0)
            return;

        m_time += Time.deltaTime;
        if(m_time > m_timeToReduce)
            m_cutOutFactor += Time.deltaTime * m_reduceFactor;

        if (m_time <= m_timeToStart)
            m_cutOutFactor = 1.0f;
        else if (m_time > m_timeToStart && m_time <= m_timeToReduce)
            m_cutOutFactor -= Time.deltaTime*m_startFactor;

        m_cutOutFactor = Mathf.Clamp01(m_cutOutFactor);
        if (m_cutOutFactor >= 1 && m_time > m_timeToReduce)
            Destroy(gameObject);
        m_objectMaterial.SetFloat("_CutOut", m_cutOutFactor);
	}

    public void SetTime(float _Duration, bool _Loop)
    {
        if (!_Loop)
            m_timeToReduce = _Duration+1;
        else
            m_timeToReduce = 0.0f;
    }
}
