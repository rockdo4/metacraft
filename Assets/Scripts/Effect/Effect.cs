using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public List<ParticleSystem> particles;
    public EffectData data;
    public Transform startPos;              // ����Ʈ ���� ����
    public Transform endPos;                // ����Ʈ ���� ����
    // ���� ���� �߰��� ����
    // ��ǥ���� �̵��ϴ� ����Ʈ ���� ���� ������ ���Ƽ� �̸� �߰�
    // ������ None���� ���� �˴ϴ�

    public void Awake()
    {
        gameObject.SetActive(false);
    }

    // ���� ���� �����ϰ� �� ���� ������ ���Ƽ� �߰�
    public void SetStartPos(Transform startPos)
    {
        this.startPos = startPos;
    }
    public void SetRotation(Quaternion rot)
    {
        gameObject.transform.rotation = rot;
    }
    // ���� ���� �����ϰ� �� ���� ������ ���Ƽ� �߰�
    public void SetEndPos(Transform endPos)
    {
        this.endPos = endPos;
    }

    // ���� �Ӽ��� �����ؼ� �� �� �ֵ��� �ϱ� ���� �߰�
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
