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
    
    private static List<(SePlayer prefab, LinkedList<SePlayer> use, LinkedList<SePlayer> unUse)> sePlayerPools;
    public List<SePlayer> prefabsFromInspector;
    
    //�̱��� ��ӹ޾����� awake �����Ҷ� ����    
    private void Start()
    {
        sePlayerPools =
            new List<(SePlayer prefab, LinkedList<SePlayer> use, LinkedList<SePlayer> unUse)>((int)SeList.TotalCount);
            
        CheckAllSEPrefabSetted();
        SetSePlayerPools();
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
    private void CheckAllSEPrefabSetted()
    {
        if (prefabsFromInspector.Count > (int)SeList.TotalCount)
        {
            Logger.Error("������ ���� ���� SE���ں��� �����ϴ�");            
        }
        LinkedList<int> list = new LinkedList<int>();
        for (int i = 0; i < (int)SeList.TotalCount; ++i)
        {
            list.AddLast(i);
        }
        for (int i = 0; i < prefabsFromInspector.Count; ++i)
        {
            list.Remove((int)prefabsFromInspector[i].ClipName);
        }
        if(list.Count != 0)
        {
            foreach(var num in list)
            {
                Logger.Error($"{num}�� SE���尡 ���Ե��� �ʾҽ��ϴ�. SeList�� �����ϼ���");
            }
        }
    }
    private void SetSePlayerPools()
    {
        prefabsFromInspector.Sort((a, b) => ((int)a.ClipName).CompareTo((int)b.ClipName));

        for(int i = 0; i < sePlayerPools.Count; i++)
        {

        }     
    }
    public void PlaySE(SeList clipName)
    {
        if(sePlayerPools[(int)clipName].use.Count == 0)
        {

        }
    }
}
