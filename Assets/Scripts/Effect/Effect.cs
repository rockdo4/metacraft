using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public EffectData data;
    private ParticleSystem particle;
    public Transform startPos;              // 이펙트 시작 지점
    public Transform endPos;                // 이펙트 종료 지점
    // 종료 지점 추가한 이유
    // 목표한테 이동하는 이펙트 생길 수도 있을거 같아서 미리 추가
    // 당장은 None으로 놔도 됩니다

    void Awake()
    {
        particle = data.particle;
        gameObject.SetActive(false);
    }

    // 풀에 담을 용도인 메서드
    // effectName은 없어도 되면 삭제할 예정 (보기 편하려고 추가)
    public Effect CreateEffect(Transform parentsObject, string effectName)
    {
        var effect = Instantiate(this, startPos.position, Quaternion.identity, parentsObject);
        effect.name = effectName;
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

    // 풀 안의 이펙트들마다 특정 상황에 각각 속성들을 변경해서 쓸 수 있도록 하기 위해 추가
    // 필요 없을 시 제거해도 됨
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
