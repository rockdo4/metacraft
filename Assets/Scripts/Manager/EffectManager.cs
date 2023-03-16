using System.Collections.Generic;
using UnityEngine;

public class EffectManager : EffectManagerSingleton<EffectManager>
{
    public int effectPoolSize;
    public List<Effect> effectList;      // 생성할 이펙트 리스트들

    public List<List<Effect>> effectPool = new();    // 이펙트 풀
    private List<int> effectPoolIndex = new();     // 활성화할 이펙트 프리펩의 번호
    private GameObject parent;

    private EffectEnum currEffect;
    public void Get(EffectEnum index, Transform startPos, Quaternion rot = default)
    {
        if (index.Equals(EffectEnum.None))
            return;

        if (effectPool.Count == 0)
        {
            GameObject gm = new("Effects");
            parent = Instantiate(gm);
            DontDestroyOnLoad(parent);
            effectPoolSize = 5;
            effectList = GameManager.Instance.effects;
            CreateAllEffects();
        }

        currEffect = index;
        
        // index = 뽑고 싶은 이펙트가 저장된 풀의 이름 (Enum)
        int poolIndex = (int)index;

        // effectPool의 [풀 번호][이펙트인덱스[번호]] 활성화
        effectPool[poolIndex][effectPoolIndex[poolIndex]].SetStartPos(startPos);
        effectPool[poolIndex][effectPoolIndex[poolIndex]].SetRotation(rot);
        effectPool[poolIndex][effectPoolIndex[poolIndex]].StartEffect();

        // index가 프리펩의 최대 개수를 넘어가면 0으로 초기화
        if (effectPoolIndex[poolIndex] < effectPool[poolIndex].Count - 1)
            effectPoolIndex[poolIndex]++;
        else
            effectPoolIndex[poolIndex] = 0;
    }

    public Effect GetCurrEffect()
    {
        var index = effectPoolIndex[(int)currEffect];
        index = index.Equals(0) ? effectPool[(int)currEffect].Count - 1 : --index;

        return effectPool[(int)currEffect][index];
    }

    // 담고있는 리스트의 모든 이펙트 생성
    private void CreateAllEffects()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            CreateSelectEffect(i);
        }
    }

    // 풀에 담을 용도
    private Effect CreateEffect(Transform parentsObject, string effectName, int effectListIndex)
    {
        var effect = Instantiate(effectList[effectListIndex], parentsObject);
        effect.name = effectName;

        var particles = effectList[effectListIndex].data.particles;

        for (int i = 0; i < particles.Count; i++)
        {
            var particleData = Instantiate(particles[i], effect.transform);

            if (effect.particles.Count == 0)
                effect.particles.Add(particleData.GetComponentInChildren<ParticleSystem>());
        }

        return effect;
    }

    // 한 종류의 이펙트들 생성
    private void CreateSelectEffect(int effectListIndex)
    {
        // "이펙트 프리펩 이름 + Pool" 이름의 빈 게임 오브젝트를 Effects 게임 오브젝트의 자식으로 생성
        // (종류별로 구분해서 담아두기 위함)
        GameObject parents = new ($"{effectList[effectListIndex].name} Pool");
        parents.transform.parent = parent.transform;

        effectPool.Add(new List<Effect>());
        effectPoolIndex.Add(0);
        for (int i = 0; i < effectPoolSize; i++)
        {
            // 이펙트 리스트 요소 당 풀의 사이즈만큼 생성 (각각 번호붙여 이름도 추가)
            var effect =
                CreateEffect(parents.transform, $"{effectList[effectListIndex].name} {i}", effectListIndex);

            // 만든 이펙트 풀에 저장
            effectPool[effectListIndex].Add(effect);
        }

        //parent.transform.parent = GameObject.FindGameObjectWithTag("ParentFloor").transform;
    }

    // 현재 사용하고 있는 풀
    public int GetCurrPoolIndex()
    {
        return effectPoolIndex[(int)currEffect];
    }

    // 현재 사용하고 있는 풀의 다음 이펙트.
    // 다음에 나올 이펙트의 설정값들을 조정해야 할 수 있을 것 같아 추가
    public void SetNextEffectProperty(float startDelay, float effectSize)
    {
        int index = (int)currEffect;

        if (effectPool[index][effectPoolIndex[index] + 1] != null)
        {
            effectPool[index][effectPoolIndex[index + 1]].
                SetAllData(startDelay, effectSize);
        }
        else
        {
            effectPool[index][effectPoolIndex[0]].SetAllData(startDelay, effectSize);
        }
    }
}
