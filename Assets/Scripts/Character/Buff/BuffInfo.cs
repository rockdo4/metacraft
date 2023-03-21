using UnityEngine;

[CreateAssetMenu(fileName = "BuffInfo", menuName = "Character/Buff/BuffInfo")]
public class BuffInfo : ScriptableObject
{
    [Header("�ߺ� ���� �Ұ�")]
    public int id;              //������ ID
    public int sort;            //������ ��ȣ, ���̺� �ִ� sort��ȣ
    public new string name;         //�����̻��� �̸�(��Ʈ�����̺� ����)
    public string info;         //�����̻��� ����(��Ʈ�����̺� ����)

    [Space, Space]
    [Header("0:Ư�� �����̻�(ex: ü��ȸ��)")]
    [Header("1: ����(��������) 2: �����(��������)")]
    [Header("3: ����(�����Ұ�) 4: �����(�����Ұ�)")]
    public int fraction;        //�����̻��� �з�(0:Ư�� �����̻�(������ ������� �ƴ�)
                                //1: ����(��������) 2: �����(��������) 3: ����(�����Ұ�) 4: �����(�����Ұ�)

    [Space, Space]
    [Header("��������")]
    public bool inf = false;    //��������


    [Space, Space]
    public BuffType type;       //�����̻��� ȿ��
    [Header("�����̻��� ȿ����")]
    public float buffValue;       //�����̻��� ȿ����
    [Header("�����̻��� ���ӽð�")]
    public float duration;      //�����̻��� ���ӽð�
}
