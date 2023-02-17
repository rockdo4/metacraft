using System.Collections.Generic;
using Unity.VisualScripting;
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
    
    private static List<(LinkedList<SePlayer> use, LinkedList<SePlayer> unUse)> sePlayerPools;    
  
    public List<SePlayer> prefabs;
    
    //싱글톤 상속받았으니 awake 구현할때 조심    
    private void Start()
    {
        sePlayerPools = new ((int)SeList.TotalCount);
        prefabs.Sort((a, b) => ((int)a.ClipName).CompareTo((int)b.ClipName));
        CheckAllSEPrefabSetted();
        InitSePlayerPoolsSetting();
    }
    private void Update()
    {        
        MixerControl();
    }
    public void MixerControl()
    {
        //매 업데이트에서 체크하고 싶지 않음
        mixer.SetFloat(nameof(master), master);
        mixer.SetFloat(nameof(bgm), bgm);
        mixer.SetFloat(nameof(se), se);
    }
    private void CheckAllSEPrefabSetted()
    {
        if (prefabs.Count > (int)SeList.TotalCount)
        {
            Logger.Error("프레펩 수가 실제 SE숫자보다 많습니다");            
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
                Logger.Error($"{num}번 SE사운드가 포함되지 않았습니다. SeList를 참조하세요");
            }
        }
    }
    private void InitSePlayerPoolsSetting()
    {        
        for (int i = 0; i < sePlayerPools.Capacity; i++)
        {
            sePlayerPools.Add((new(), new()));
        }        
    }
    public void PlaySE(SeList clipName, Transform transform)
    {
        var index = (int)clipName;

        if (sePlayerPools[index].use.Count >= prefabs[index].MaxPlayCount)
            return;

        if (sePlayerPools[index].unUse.Count == 0)
        {
            sePlayerPools[index].use.AddLast(Instantiate(prefabs[index]));
        }
        else
        {
            sePlayerPools[index].use.AddLast(sePlayerPools[index].unUse.Last);
            sePlayerPools[index].unUse.RemoveLast();
        }

        
    }
}
