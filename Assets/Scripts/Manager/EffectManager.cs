using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [Range(0, 100)]
    public int effectPoolSize;
    public List<Effect> effectList;      // 생성할 이펙트 리스트들. 인스펙터에서 추가
    public Transform parentsObject;

    private List<List<Effect>> effectPool = new((int)EffectEnum.Count);    // 이펙트 풀
    private List<int> effectPoolIndex = new();     // 활성화할 이펙트 프리펩의 번호

    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CreateAllEffects();
    }

    public void Get(EffectEnum index)
    {
        // index = 뽑고 싶은 이펙트가 저장된 풀의 이름 (Enum)

        int poolIndex = (int)index;
        // effectPool의 [풀 번호][이펙트인덱스[번호]] 활성화
        effectPool[poolIndex][effectPoolIndex[poolIndex]].StartEffect();

        // index가 프리펩의 최대 개수를 넘어가면 0으로 초기화
        if (effectPoolIndex[poolIndex] < effectPool[poolIndex].Count - 1)
            effectPoolIndex[poolIndex]++;
        else
            effectPoolIndex[poolIndex] = 0;
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
        var effect =
            Instantiate(effectList[effectListIndex],
            effectList[effectListIndex].startPos.position,
            Quaternion.identity,
            parentsObject);
        effect.name = effectName;

        var particles = effectList[effectListIndex].data.particles;
        var particleList = effect.particles;

        for (int i = 0; i < particles.Count; i++)
        {
            var particleData = Instantiate(particles[i], effect.transform);

            if (particleList.Count == 0)
                particleList.Add(particleData.GetComponentInChildren<ParticleSystem>());
        }

        return effect;
    }

    // 한 종류의 이펙트들 생성
    private void CreateSelectEffect(int effectListIndex)
    {
        // "이펙트 프리펩 이름 + Pool" 이름의 빈 게임 오브젝트를 Effects 게임 오브젝트의 자식으로 생성
        // (종류별로 구분해서 담아두기 위함)
        var parents = new GameObject($"{effectList[effectListIndex].name} Pool");
        parents.transform.parent = parentsObject.transform;

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
    }
}
