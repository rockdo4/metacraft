using System.Collections.Generic;
using UnityEngine;

public class EffectManager : EffectManagerSingleton<EffectManager>
{
    public int effectPoolSize;
    public List<Effect> effectList;      // ������ ����Ʈ ����Ʈ��

    public List<List<Effect>> effectPool = new();    // ����Ʈ Ǯ
    private List<int> effectPoolIndex = new();     // Ȱ��ȭ�� ����Ʈ �������� ��ȣ
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
        
        // index = �̰� ���� ����Ʈ�� ����� Ǯ�� �̸� (Enum)
        int poolIndex = (int)index;

        // effectPool�� [Ǯ ��ȣ][����Ʈ�ε���[��ȣ]] Ȱ��ȭ
        effectPool[poolIndex][effectPoolIndex[poolIndex]].SetStartPos(startPos);
        effectPool[poolIndex][effectPoolIndex[poolIndex]].SetRotation(rot);
        effectPool[poolIndex][effectPoolIndex[poolIndex]].StartEffect();

        // index�� �������� �ִ� ������ �Ѿ�� 0���� �ʱ�ȭ
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

    // �� ������ ����Ʈ�� ����
    private void CreateSelectEffect(int effectListIndex)
    {
        // "����Ʈ ������ �̸� + Pool" �̸��� �� ���� ������Ʈ�� Effects ���� ������Ʈ�� �ڽ����� ����
        // (�������� �����ؼ� ��Ƶα� ����)
        GameObject parents = new ($"{effectList[effectListIndex].name} Pool");
        parents.transform.parent = parent.transform;

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

        //parent.transform.parent = GameObject.FindGameObjectWithTag("ParentFloor").transform;
    }

    // ���� ����ϰ� �ִ� Ǯ
    public int GetCurrPoolIndex()
    {
        return effectPoolIndex[(int)currEffect];
    }

    // ���� ����ϰ� �ִ� Ǯ�� ���� ����Ʈ.
    // ������ ���� ����Ʈ�� ���������� �����ؾ� �� �� ���� �� ���� �߰�
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
