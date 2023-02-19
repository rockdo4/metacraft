using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    public AudioMixer mixer;

    [Range(-80, 0)]
    public float master = 0;

    [Range(-80, 0)]
    public float bgm = 0;

    [Range(-80, 0)]
    public float se = 0;
        
    private static List<(Queue<SePlayer> use, Queue<SePlayer> unUse)> sePlayerPools;    
  
    public List<SePlayer> prefabs;
    
    //�̱��� ��ӹ޾����� awake �����Ҷ� ����    
    private void Start()
    {
        sePlayerPools = new ((int)SeList.TotalCount);
        prefabs.Sort((a, b) => ((int)a.ClipName).CompareTo((int)b.ClipName));
        CheckAllSEprefabSetted();
        InitSEplayerPoolsSetting();
    }
    private void Update()
    {        
        MixerControl();
    }
    public void MixerControl()
    {
        //�� ������Ʈ���� üũ�ϰ� ���� ����
        mixer.SetFloat(nameof(master), master);
        mixer.SetFloat(nameof(bgm), bgm);
        mixer.SetFloat(nameof(se), se);
    }
    private void CheckAllSEprefabSetted()
    {
        if (prefabs.Count > (int)SeList.TotalCount)
        {
            Logger.Error("������ ���� ���� SE���ں��� �����ϴ�");            
        }
        LinkedList<int> list = new();
        for (int i = 0; i < (int)SeList.TotalCount; ++i)
        {
            list.AddLast(i);
        }
        for (int i = 0; i < prefabs.Count; ++i)
        {
            list.Remove((int)prefabs[i].ClipName);
        }
        if(list.Count != 0)
        {
            foreach(var num in list)
            {
                Logger.Error($"{num + 1}�� SE���尡 ���Ե��� �ʾҽ��ϴ�. SeList�� �����ϼ���");
            }
        }
    }
    private void InitSEplayerPoolsSetting()
    {        
        for (int i = 0; i < sePlayerPools.Capacity; i++)
        {
            sePlayerPools.Add((new(), new()));
        }        
    }
    public static void PlaySE(SeList clipName)
    {
        var index = (int)clipName;

        if (sePlayerPools[index].use.Count >= Instance.prefabs[index].MaxPlayCount)
            return;

        SePlayer se;

        if (sePlayerPools[index].unUse.Count == 0)
        {
            se = Instantiate(Instance.prefabs[index], Instance.transform);
            se.SetPool(sePlayerPools[index].use, sePlayerPools[index].unUse);

            sePlayerPools[index].use.Enqueue(se);            
        }
        else
        {
            se = sePlayerPools[index].unUse.Dequeue();
            sePlayerPools[index].use.Enqueue(se);            
        }

        se.PlaySE();
    }
    public static void PlaySE(SeList clipName, Transform transform)
    {

    }
}
