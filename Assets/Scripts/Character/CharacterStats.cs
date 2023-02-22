using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Character/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public int Power
    {
        // ������ ����
        private set { power = baseDamage + healthPoint; }
        get { return power; }
    }
    
    private int             power;              // ������
    public int              baseDamage = 50;    // �Ϲ� ���� ������
    public int              baseDefense = 0;    // ����
    public int              healthPoint = 500;  // �ִ� ü��
    public int              moveSpeed;          // �̵� �ӵ�. ����, �ʱⰪ ���� �ʿ�
    [Range(0f, 1f)]
    public float            critical = 0f;      // ũ��Ƽ�� Ȯ��
    [Range(1f, 10f)]                            
    public float            criticalDmg = 2f;   // ũ��Ƽ�� ������ ����
    [Range(0f, 1f)]                             
    public float            evasion = 0f;       // ȸ����
    [Range(0f, 1f)]                             
    public float            accuracy = 0.5f;    // ���߷�

    public GameObject       equipment;          // ���
    public List<int>        synergys;           // �ó��� ����Ʈ. ���� or Enum���� ������ �ִٰ�
                                                // ���ݴ뿡 ���� �ó��� index�� ������ �ִ� ������ ������ �ó��� �ߵ�
}