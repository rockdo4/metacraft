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
    public static List<LinkedList<(SePlayer use, SePlayer unuse)>> sePlayersPool;
    public int SePlayersCount { get; set; } = 0;
    public int SePlayersMaxCount = 30;

    //�̱��� ��ӹ޾����� awake �����Ҷ� ����    
    private void Start()
    {
        //sePlayersPool = new List<LinkedList<(SePlayer use, SePlayer unuse)>>((int));
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
}
