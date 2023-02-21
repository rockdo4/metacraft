using System.Collections.Generic;
using UnityEngine;

// ����Ʈ ������ ������ ��ũ���ͺ������Ʈ
[CreateAssetMenu(fileName = "Effect", menuName = "Effect/EffectData")]
public class EffectData : ScriptableObject
{
    public List<GameObject> particles;
    public float startDelay;        // ����Ʈ ���� �� ������
    public float duration;          // ���ӽð�
    public float effectSize;        // ����Ʈ ũ��
    public float timeScale;         // ��� �ӵ�
}
