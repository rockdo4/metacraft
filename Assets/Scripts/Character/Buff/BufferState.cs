using System;
using UnityEngine;

[SerializeField]
public class BufferState
{
    public float damage = 1;         // ���ݷ�
    public float Damage => Math.Clamp(damage, -0.5f, 3f);

    public float defense = 1;      // ����
    public float damageReceived = 1;      // �޴� ����
    public float damageDecrease = 1;      // �ִ� ����
    public float criticalProbability = 0;      // ġ��Ÿ Ȯ�� 
    public float criticalDamage = 1;      // ġ��Ÿ ���ط� ����
    public float attackSpeed = 1;      // ���ݼӵ� ����
    public float maxHealthIncrease = 1;      // �ִ� ü�� ����

    public float freeze = 0;      // ����
    public bool isFreze = false;

    public float shield = 0;      // ��ȣ��
    public bool isShield = false;

    public bool provoke = false; // ����
    public bool stealth = false; // ����
    public bool stun = false; // ����
    public bool silence = false; // ħ��
    public int energyCnt = 0;

    public void Buffer(BuffType type, BuffInfo info, bool isRemove = false, bool isSet = true)
    {
        var scale = (info.buffValue + ((info.buffLevel - 1) * info.addBuffState)) * (isRemove ? -1f : 1f);
        switch (type)
        {
            case BuffType.PowerUp:
                damage += scale;
                break;
            case BuffType.PowerDown:
                damage -= scale;
                break;
            case BuffType.DefenseUp:
                defense += scale;
                break;
            case BuffType.DefenseDown:
                defense -= scale;
                break;
            case BuffType.CriticalProbabilityUp:
                criticalProbability += scale;
                break;
            case BuffType.CriticalProbabilityDown:
                criticalProbability -= scale;
                break;
            case BuffType.AttackSpeedUp:
                attackSpeed += scale;
                break;
            case BuffType.AttackSpeedDown:
                attackSpeed -= scale;
                break;
            case BuffType.MaxHealthIncrease:
                maxHealthIncrease += scale;
                break;
            case BuffType.Shield:
                shield += scale;
                isShield = isSet;
                break;
            case BuffType.Heal:
                break;
            case BuffType.Provoke:
                provoke = isSet;
                break;
            case BuffType.Stun:
                stun = isSet;
                break;
            case BuffType.Silence:
                silence = isSet;
                break;
            default:
                break;
        }
    }
    public void Buffer(BuffType type, int value)
    {
        switch (type)
        {
            case BuffType.energyCharging:
                energyCnt += value;
                break;
            default:
                break;
        }
    }
    public void RemoveBuffer(BuffType type, BuffInfo info)
    {
        Buffer(type, info, true, false);
    }
}