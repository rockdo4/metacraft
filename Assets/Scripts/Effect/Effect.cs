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
    // ���� ���� �����ϰ� �� ���� ������ ���Ƽ� �߰�
    public void SetEndPos(Transform endPos)
    {
        this.endPos = endPos;
    }

    // ���� �Ӽ��� �����ؼ� �� �� �ֵ��� �ϱ� ���� �߰�
    public void SetAllData
        (float startDelay, float duration, float effectSize)
    {
        data.startDelay = startDelay;
        data.duration = duration;
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
        var duration = data.duration;

        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < particles.Count; i++)
        {
            if (particles[i] == null)
                Logger.Debug("null");
            particles[i].Play();
        }

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
    // ��ƼŬ ��ü�� ���ӽð����� ���߰� ����� ���� ����
}
