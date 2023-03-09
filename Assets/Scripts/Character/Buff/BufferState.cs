
public class BufferState
{
    public float power = 0;         // ���ݷ�
    public float defense = 0;      // ����
    public float damageReceived = 0;      // �޴� ����
    public float damageDecrease = 0;      // �ִ� ����
    public float criticalProbability = 0;      // ġ��Ÿ Ȯ�� 
    public float criticalDamage = 0;      // ġ��Ÿ ���ط� ����
    public float attackSpeed = 0;      // ���ݼӵ� ����
    public float maxHealthIncrease = 0;      // �ִ� ü�� ����

    public float burns = 0;      // ȭ��
    public bool isBurns = false;

    public float freeze = 0;      // ����
    public bool isFreze = false;

    public float shield = 0;      // ��ȣ��
    public bool isShield = false;

    public float bleed = 0;      // ����
    public bool isBleed = false;

    public float healthRegen = 0;      // ü�� ȸ��
    public bool isHealtRegen = false;

    public float lifeSteal = 0;      // ����
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
                power += scale;
                break;
            case BuffType.PowerDown:
                power -= scale;
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
            case BuffType.HealthRegen:
                healthRegen += scale;
                isHealtRegen = isSet;
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