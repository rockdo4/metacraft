
using System;

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

    public float burns = 0;      // ȭ��
    public bool isBurns = false;

    public float freeze = 0;      // ����
    public bool isFreze = false;

    public float shield = 0;      // ��ȣ��
    public bool isShield = false;

    public float bleed = 0;      // ����
    public bool isBleed = false;

    public float lifeSteal = 1;      // ����
    public bool isLifeSteal = false;

    public bool provoke = false; // ����
    public bool stealth = false; // ����
    public bool stun = false; // ����
    public bool silence = false; // ħ��
    public bool resistance = false; // ����
    public bool blind = false; // �Ǹ�

    public void Buffer(BuffType type, float scale, bool isSet = true)
    {
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
            case BuffType.DamageReceivedUp:
                damageReceived += scale;
                break;
            case BuffType.DamageReceivedDown:
                damageReceived -= scale;
                break;
            case BuffType.DamageDecreaseUp:
                damageDecrease += scale;
                break;
            case BuffType.DamageDecreaseDown:
                damageDecrease -= scale;
                break;
            case BuffType.CriticalProbabilityUp:
                criticalProbability += scale;
                break;
            case BuffType.CriticalProbabilityDown:
                criticalProbability -= scale;
                break;
            case BuffType.CriticalDamageUp:
                criticalDamage += scale;
                break;
            case BuffType.CriticalDamageDown:
                criticalDamage -= scale;
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
            case BuffType.Burns:
                burns += scale;
                isBurns = isSet;
                break;
            case BuffType.Freeze:
                freeze += scale;
                isFreze = isSet;
                break;
            case BuffType.Shield:
                shield += scale;
                isShield = isSet;
                break;
            case BuffType.Bleed:
                bleed += scale;
                isBleed = isSet;
                break;
            case BuffType.Heal:
                break;
            case BuffType.LifeSteal:
                lifeSteal += scale;
                isLifeSteal = isSet;
                break;
            case BuffType.Provoke:
                provoke = isSet;
                break;
            case BuffType.Stealth:
                stealth = isSet;
                break;
            case BuffType.Stun:
                stun = isSet;
                break;
            case BuffType.Silence:
                silence = isSet;
                break;
            case BuffType.Resistance:
                resistance = isSet;
                break;
            case BuffType.Blind:
                blind = isSet;
                break;
            default:
                break;
        }
    }
    public void RemoveBuffer(BuffType type, float scale)
    {
        Buffer(type, -scale, false);
    }
}