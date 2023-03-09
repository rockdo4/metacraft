
public class BufferState
{
    public int power = 0;         // ���ݷ�
    public int defense = 0;      // ����
    public int damageReceived = 0;      // �޴� ����
    public int damageDecrease = 0;      // �ִ� ����
    public int criticalProbability = 0;      // ġ��Ÿ Ȯ�� 
    public int criticalDamage = 0;      // ġ��Ÿ ���ط� ����
    public int attackSpeed = 0;      // ���ݼӵ� ����
    public int maxHealthIncrease = 0;      // �ִ� ü�� ����

    public int burns = 0;      // ȭ��
    public bool isBurns = false;

    public int freeze = 0;      // ����
    public bool isFreze = false;

    public int shield = 0;      // ��ȣ��
    public bool isShield = false;

    public int bleed = 0;      // ����
    public bool isBleed = false;

    public int healthRegen = 0;      // ü�� ȸ��
    public bool isHealtRegen = false;

    public int lifeSteal = 0;      // ����
    public bool isLifeSteal = false;

    public bool provoke = false; // ����
    public bool stealth = false; // ����
    public bool stun = false; // ����
    public bool silence = false; // ħ��
    public bool resistance = false; // ����
    public bool blind = false; // �Ǹ�

    public void Buffer(BuffType type, int scale, bool isSet = true)
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
    public void RemoveBuffer(BuffType type, int scale)
    {
        Buffer(type, -scale, false);
    }
}