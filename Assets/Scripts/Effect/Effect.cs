using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public List<ParticleSystem> particles;
    public EffectData data;
    public Transform startPos;              // 이펙트 시작 지점
    public Transform endPos;                // 이펙트 종료 지점
    // 종료 지점 추가한 이유
    // 목표한테 이동하는 이펙트 생길 수도 있을거 같아서 미리 추가
    // 당장은 None으로 놔도 됩니다

    public void Awake()
    {
        gameObject.SetActive(false);
    }

    // 시작 지점 변경하게 될 수도 있을거 같아서 추가
    public void SetStartPos(Transform startPos)
    {
        this.startPos = startPos;
    }
    public void SetRotation(Quaternion rot)
    {
        gameObject.transform.rotation = rot;
    }
    // 종료 지점 변경하게 될 수도 있을거 같아서 추가
    public void SetEndPos(Transform endPos)
    {
        this.endPos = endPos;
    }

    // 각각 속성들 변경해서 쓸 수 있도록 하기 위해 추가
    public void SetAllData
        (float startDelay, float effectSize)
    {
        data.startDelay = startDelay;
        data.effectSize = effectSize;
    }

    public void StartEffect()
    {
        gameObject.SetActive(true);
        StartCoroutine(CoEffectRoutine());
    }

    IEnumerator CoEffectRoutine()
    {        
        var startDelay = data.startDelay;
        float duration = particles[0].main.duration;

        while (startDelay > 0f)
        {
            startDelay -= Time.deltaTime;
            yield return null;
        }    

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].transform.position = startPos.position;
            particles[i].gameObject.SetActive(true);
            particles[i].Play();
        }

        yield return null;

        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }

        gameObject.SetActive(false);
    }
}
