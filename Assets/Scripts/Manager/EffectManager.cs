using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [Range(0, 100)]
    public int effectPoolSize;
    public List<Effect> effectList;      // ������ ����Ʈ ����Ʈ��. �ν����Ϳ��� �߰�
    public Transform parentsObject;

    private List<List<Effect>> effectPool = new((int)EffectEnum.Count);    // ����Ʈ Ǯ
    private List<int> effectPoolIndex = new();     // Ȱ��ȭ�� ����Ʈ �������� ��ȣ

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
        // index = �̰� ���� ����Ʈ�� ����� Ǯ�� �̸� (Enum)

        int poolIndex = (int)index;
        // effectPool�� [Ǯ ��ȣ][����Ʈ�ε���[��ȣ]] Ȱ��ȭ
        effectPool[poolIndex][effectPoolIndex[poolIndex]].StartEffect();

        // index�� �������� �ִ� ������ �Ѿ�� 0���� �ʱ�ȭ
        if (effectPoolIndex[poolIndex] < effectPool[poolIndex].Count - 1)
            effectPoolIndex[poolIndex]++;
        else
            effectPoolIndex[poolIndex] = 0;
    }

    // ����ִ� ����Ʈ�� ��� ����Ʈ ����
    private void CreateAllEffects()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            CreateSelectEffect(i);
        }
    }

    // Ǯ�� ���� �뵵
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

    // �� ������ ����Ʈ�� ����
    private void CreateSelectEffect(int effectListIndex)
    {
        // "����Ʈ ������ �̸� + Pool" �̸��� �� ���� ������Ʈ�� Effects ���� ������Ʈ�� �ڽ����� ����
        // (�������� �����ؼ� ��Ƶα� ����)
        var parents = new GameObject($"{effectList[effectListIndex].name} Pool");
        parents.transform.parent = parentsObject.transform;

        effectPool.Add(new List<Effect>());
        effectPoolIndex.Add(0);
        for (int i = 0; i < effectPoolSize; i++)
        {
            // ����Ʈ ����Ʈ ��� �� Ǯ�� �����ŭ ���� (���� ��ȣ�ٿ� �̸��� �߰�)
            var effect =
                CreateEffect(parents.transform, $"{effectList[effectListIndex].name} {i}", effectListIndex);

            // ���� ����Ʈ Ǯ�� ����
            effectPool[effectListIndex].Add(effect);
        }
    }
}
