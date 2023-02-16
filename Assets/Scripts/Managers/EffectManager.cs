using Chapter.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Range(0, 100)]
    public int effectPoolSize;
    public List<Effect> effectList;      // 생성할 이펙트 리스트들. 인스펙터에서 추가
    public Transform parentsObject;
    private static List<List<Effect>> effectPool = new List<List<Effect>>();    // 이펙트 풀
    private static int effectIndex = 0;
    private static List<int> effectPoolIndex = new List<int>();     // 활성화할 이펙트 프리펩의 번호

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
        // index = 뽑고 싶은 이펙트가 저장된 풀의 번호
        effectIndex = index;
        // effectPool의 [풀 번호][이펙트인덱스[번호]] 활성화
        effectPool[effectIndex][effectPoolIndex[effectIndex]].gameObject.SetActive(true);

        // index가 프리펩의 최대 개수를 넘어가면 0으로 초기화
        if (effectPoolIndex[effectIndex] < effectPool[effectIndex].Count - 1)
            effectPoolIndex[effectIndex]++;
        else
            effectPoolIndex[effectIndex] = 0;
    }

    // 담고있는 리스트의 모든 이펙트 생성
    private void CreateAllEffects()
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            CreateSelectEffect(i);
        }
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
            var effect = effectList[effectListIndex].
                CreateEffect(parents.transform, $"{effectList[effectListIndex].name} {i}");

            // 만든 이펙트 풀에 저장
            effectPool[effectListIndex].Add(effect);
        }
    }
}
