using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Range(0, 100)]
    public int effectPoolSize;
    public List<Effect> effectList;      // ������ ����Ʈ ����Ʈ��. �ν����Ϳ��� �߰�
    public Transform parentsObject;
    private static List<List<Effect>> effectPool = new List<List<Effect>>();    // ����Ʈ Ǯ
    public static EffectEnum effectIndex = EffectEnum.None;
    private static List<int> effectPoolIndex = new List<int>();     // Ȱ��ȭ�� ����Ʈ �������� ��ȣ

    private void Start()
    {
        CreateAllEffects();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Get(EffectEnum.Test1);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Get(EffectEnum.Test2);
        }
    }

    public static void Get(EffectEnum index)
    {
        // index = �̰� ���� ����Ʈ�� ����� Ǯ�� �̸� (Enum)
        if (effectIndex != index)
        {
            effectIndex = index;
        }

        int poolIndex = (int)effectIndex;
        // effectPool�� [Ǯ ��ȣ][����Ʈ�ε���[��ȣ]] Ȱ��ȭ
        effectPool[poolIndex][effectPoolIndex[poolIndex]].StartEffect();
        //effectPool[effectIndex].Dequeue().StartEffect();

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
