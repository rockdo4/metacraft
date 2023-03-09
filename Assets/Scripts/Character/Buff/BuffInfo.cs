using UnityEngine;

[CreateAssetMenu(fileName = "BuffInfo", menuName = "Character/Buff/BuffInfo")]
public class BuffInfo : ScriptableObject
{
    public string id;
    public BuffType type;

    public bool inf = false;    //영구지속
    public float duration;      //지속시간
    public float buffScale;     //value
    public Effect buffParticle; //생성 파티클
}
