using UnityEngine;

[CreateAssetMenu(fileName = "SkillBase", menuName = "SkillBase/SkillBase")]
public class SkillBase : ScriptableObject
{
    public float cooldown;          // ��ٿ� ���� �ð�
    public float maxCooldown;       // ��ٿ�
    public float distance;          // ���� ���� (= �׺���̼� ���� �Ÿ�)
    public int count;               // ��ų �ִ� ���� ��ü��
    public float angle;             // ���� ���� ��

    // ���� �̿� �����, ������ ���� �� �ֱ� ������ �������� �����
    // �Ϲ� ��ų(��Ÿ)�� �ش� Ŭ���� ���ؼ� �ۼ�
}