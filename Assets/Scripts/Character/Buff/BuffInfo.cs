using UnityEngine;

[CreateAssetMenu(fileName = "BuffInfo", menuName = "Character/Buff/BuffInfo")]
public class BuffInfo : ScriptableObject
{
    [Header("중복 절대 불가")]
    public int id;              //상태의 ID
    public int sort;            //아이콘 번호, 테이블에 있는 sort번호
    public new string name;         //상태이상의 이름(스트링테이블 연결)
    public string info;         //상태이상의 설명(스트링테이블 연결)

    [Space, Space]
    [Header("0:특수 상태이상(ex: 체력회복)")]
    [Header("1: 버프(해제가능) 2: 디버프(해제가능)")]
    [Header("3: 버프(해제불가) 4: 디버프(해제불가)")]
    public int fraction;        //상태이상의 분류(0:특수 상태이상(버프도 디버프도 아님)
                                //1: 버프(해제가능) 2: 디버프(해제가능) 3: 버프(해제불가) 4: 디버프(해제불가)

    [Space, Space]
    [Header("영구지속")]
    public bool inf = false;    //영구지속


    [Space, Space]
    public BuffType type;       //상태이상의 효과
    [Header("상태이상의 효과값")]
    public float buffValue;       //상태이상의 효과값
    [Header("상태이상의 지속시간")]
    public float duration;      //상태이상의 지속시간
}
