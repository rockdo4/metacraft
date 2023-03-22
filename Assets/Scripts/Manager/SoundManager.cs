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

    [Range(-80, 0)]
    public float ambience = 0;

    private static List<(Queue<SePlayer> use, Queue<SePlayer> unUse)> sePlayerPools;
  
    public List<SePlayer> prefabs;
    
    //�̱��� ��ӹ޾����� awake �����Ҷ� ����    
    private void Start()
    {
        sePlayerPools = new (prefabs.Count);
        prefabs.Sort((a, b) => ((int)a.ClipName).CompareTo((int)b.ClipName));
        CheckAllSEplayerSetted();
        InitSetting();
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
        mixer.SetFloat(nameof(ambience), ambience);
    }
    private void CheckAllSEplayerSetted()
    {
        if (prefabs.Count > (int)SeList.Count)
        {
            Logger.Error("������ ���� ���� SE���ں��� �����ϴ�");            
        }
        LinkedList<int> unUsdedIndex = new();
        for (int i = 0; i < (int)SeList.Count; ++i)
        {
            unUsdedIndex.AddLast(i);
        }
        for (int i = 0; i < prefabs.Count; ++i)
        {
            int num = (int)prefabs[i].ClipName;
            if (!unUsdedIndex.Remove(num))
            {
                Logger.Error($"{num + 1}�� SE���尡 �ߺ� ���Ե� �ֽ��ϴ�. SeList�� �����ϼ���");
            }
        }
        if(unUsdedIndex.Count != 0)
        {
            foreach(var num in unUsdedIndex)
            {
                Logger.Error($"{num + 1}�� SE���尡 ���Ե��� �ʾҽ��ϴ�. SeList�� �����ϼ���");
            }
        }
    }
    private void InitSetting()
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

        SePlayer sePlayer;

        if(sePlayerPools[index].unUse.Count > 0)
        {
            sePlayer = sePlayerPools[index].unUse.Dequeue();
        }
        else
        {
            sePlayer = Instantiate(Instance.prefabs[index], Instance.transform);
            sePlayer.SetPool(sePlayerPools[index]);
        }

        sePlayerPools[index].use.Enqueue(sePlayer);
        sePlayer.PlaySE();
    }
}
