using UnityEngine;

[CreateAssetMenu(fileName = "BuffInfo", menuName = "Character/Buff/BuffInfo")]
public class BuffInfo : ScriptableObject
{
    public string id;
    public BuffType type;

    public bool inf = false;    //��������
    public float duration;      //���ӽð�
    public float buffScale;     //value
    public Effect buffParticle; //���� ��ƼŬ
}
