using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ClipInfo
{
    public SceneIndex type;
    public string subtype;
    public int id;
    public string name;
    public AudioCategory category;
    public string fileName;
    public AudioClip audioClip;
    public string clipDescription;
}

[CreateAssetMenu(fileName = "AudioTable", menuName = "Audio/AudioTable")]
public class AudioTable : ScriptableObject
{
    public List<ClipInfo> clipInfoList;

    public Dictionary<int, AudioClip> keyIDvalueClipPair = new Dictionary<int, AudioClip>();
}
