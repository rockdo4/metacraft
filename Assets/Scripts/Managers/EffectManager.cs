using Chapter.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Range(0, 100)]
    public int effectPoolSize;
    public List<Effect> effectList;      // ������ ����Ʈ ����Ʈ��. �ν����Ϳ��� �߰�
    public Transform parentsObject;
    private static List<List<Effect>> effectPool = new List<List<Effect>>();    // ����Ʈ Ǯ
    private static int effectIndex = 0;
    private static List<int> effectPoolIndex = new List<int>();     // Ȱ��ȭ�� ����Ʈ �������� ��ȣ

    private void Start()
    {
        CreateAllEffects();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Get(0);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Get(1);
        }
    }

    public static void Get(int index)
    {
        // index = �̰� ���� ����Ʈ�� ����� Ǯ�� ��ȣ
        effectIndex = index;
        // effectPool�� [Ǯ ��ȣ][����Ʈ�ε���[��ȣ]] Ȱ��ȭ
        effectPool[effectIndex][effectPoolIndex[effectIndex]].gameObject.SetActive(true);

        // index�� �������� �ִ� ������ �Ѿ�� 0���� �ʱ�ȭ
        if (effectPoolIndex[effectIndex] < effectPool[effectIndex].Count - 1)
            effectPoolIndex[effectIndex]++;
        else
            effectPoolIndex[effectIndex] = 0;
    }

    // ����ִ� ����Ʈ�� ��� ����Ʈ ����
    private void CreateAllEffects()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            CreateSelectEffect(i);
        }
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
            var effect = effectList[effectListIndex].
                CreateEffect(parents.transform, $"{effectList[effectListIndex].name} {i}");

            // ���� ����Ʈ Ǯ�� ����
            effectPool[effectListIndex].Add(effect);
        }
    }
}
