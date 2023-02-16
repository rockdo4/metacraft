using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public EffectData data;
    private ParticleSystem particle;
    public Transform startPos;              // ����Ʈ ���� ����
    public Transform endPos;                // ����Ʈ ���� ����
    // ���� ���� �߰��� ����
    // ��ǥ���� �̵��ϴ� ����Ʈ ���� ���� ������ ���Ƽ� �̸� �߰�
    // ������ None���� ���� �˴ϴ�

    void Awake()
    {
        particle = data.particle;
        gameObject.SetActive(false);
    }

    // Ǯ�� ���� �뵵�� �޼���
    // effectName�� ��� �Ǹ� ������ ���� (���� ���Ϸ��� �߰�)
    public Effect CreateEffect(Transform parentsObject, string effectName)
    {
        var effect = Instantiate(this, startPos.position, Quaternion.identity, parentsObject);
        effect.name = effectName;
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

    // Ǯ ���� ����Ʈ�鸶�� Ư�� ��Ȳ�� ���� �Ӽ����� �����ؼ� �� �� �ֵ��� �ϱ� ���� �߰�
    // �ʿ� ���� �� �����ص� ��
    public void SetAllData
        (ParticleSystem particle, float startDelay, float duration, float effectSize)
    {
        this.particle = particle;
        data.startDelay = startDelay;
        data.duration = duration;
        data.effectSize = effectSize;
    }

    private void OnEnable()
    {
        StartCoroutine(CoEffectRoutine());
    }

    IEnumerator CoEffectRoutine()
    {
        if (data.startDelay > 0f)
            yield return new WaitForSeconds(data.startDelay);
        gameObject.SetActive(true);
        particle.Play();

        var duration = data.duration;
        while (duration >= 0f)
        {
            duration -= Time.deltaTime;
            yield return 0;
        }
        particle.Stop();
        gameObject.SetActive(false);
    }
}
