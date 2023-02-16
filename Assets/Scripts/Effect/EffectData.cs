using UnityEngine;

// ����Ʈ ������ ������ ��ũ���ͺ������Ʈ
[CreateAssetMenu(fileName = "Effect", menuName = "Effect/EffectData")]
public class EffectData : ScriptableObject
{
    public ParticleSystem particle;
    public float startDelay;        // ����Ʈ ���� �� ������
    public float duration;          // ���ӽð�
    public float effectSize;        // ����Ʈ ũ��
}
