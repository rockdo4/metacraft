using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private ParticleSystem particle;
    public EffectData data;
    public Transform startPos;              // 이펙트 시작 지점
    public Transform endPos;                // 이펙트 종료 지점
    // 종료 지점 추가한 이유
    // 목표한테 이동하는 이펙트 생길 수도 있을거 같아서 미리 추가
    // 당장은 None으로 놔도 됩니다

    public void Awake()
    {
        //var dataParticle = data.particle.GetComponent<ParticleSystem>();

        //gameObject.AddComponent<ParticleSystem>();
        //particle = GetComponent<ParticleSystem>();
        //particle = dataParticle;

        gameObject.SetActive(false);
    }

    // 풀에 담을 용도
    // effectName은 없어도 되면 삭제할 예정 (보기 편하려고 추가)
    public Effect CreateEffect(Transform parentsObject, string effectName)
    {
        var effect = Instantiate(this, startPos.position, Quaternion.identity, parentsObject);
        effect.name = effectName;

        var particleData = Instantiate(data.particle, effect.gameObject.transform);
        particle = particleData.GetComponent<ParticleSystem>();

        return effect;
    }

    // 시작 지점 변경하게 될 수도 있을거 같아서 추가
    public void SetStartPos(Transform startPos)
    {
        this.startPos = startPos;
    }
    // 종료 지점 변경하게 될 수도 있을거 같아서 추가
    public void SetEndPos(Transform endPos)
    {
        this.endPos = endPos;
    }

    // 각각 속성들 변경해서 쓸 수 있도록 하기 위해 추가
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
    // 파티클 자체의 지속시간으로 멈추게 변경될 수도 있음
}
