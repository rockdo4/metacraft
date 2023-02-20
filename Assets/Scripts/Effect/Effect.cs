using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private ParticleSystem particle;
    public EffectData data;
    public Transform startPos;              // ����Ʈ ���� ����
    public Transform endPos;                // ����Ʈ ���� ����
    // ���� ���� �߰��� ����
    // ��ǥ���� �̵��ϴ� ����Ʈ ���� ���� ������ ���Ƽ� �̸� �߰�
    // ������ None���� ���� �˴ϴ�

    public void Awake()
    {
        //var dataParticle = data.particle.GetComponent<ParticleSystem>();

        //gameObject.AddComponent<ParticleSystem>();
        //particle = GetComponent<ParticleSystem>();
        //particle = dataParticle;

        gameObject.SetActive(false);
    }

    // Ǯ�� ���� �뵵
    // effectName�� ��� �Ǹ� ������ ���� (���� ���Ϸ��� �߰�)
    public Effect CreateEffect(Transform parentsObject, string effectName)
    {
        var effect = Instantiate(this, startPos.position, Quaternion.identity, parentsObject);
        effect.name = effectName;

        var particleData = Instantiate(data.particle, effect.gameObject.transform);
        particle = particleData.GetComponent<ParticleSystem>();

        return effect;
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

        if (particle == null)
        {
            Debug.LogError("particle is null");
            yield break;
        }

        particle.Play();

        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        particle.Stop();

        gameObject.SetActive(false);
    }
    // ��ƼŬ ��ü�� ���ӽð����� ���߰� ����� ���� ����
}
