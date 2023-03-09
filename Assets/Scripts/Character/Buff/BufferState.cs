
public class BufferState
{
    public int power = 0;         // 공격력
    public int defense = 0;      // 방어력
    public int damageReceived = 0;      // 받는 피해
    public int damageDecrease = 0;      // 주는 피해
    public int criticalProbability = 0;      // 치명타 확률 
    public int criticalDamage = 0;      // 치명타 피해량 증가
    public int attackSpeed = 0;      // 공격속도 증가
    public int maxHealthIncrease = 0;      // 최대 체력 증가

    public int burns = 0;      // 화상
    public bool isBurns = false;

    public int freeze = 0;      // 빙결
    public bool isFreze = false;

    public int shield = 0;      // 보호막
    public bool isShield = false;

    public int bleed = 0;      // 출혈
    public bool isBleed = false;

    public int healthRegen = 0;      // 체력 회복
    public bool isHealtRegen = false;

    public int lifeSteal = 0;      // 흡혈
    public bool isLifeSteal = false;

    public bool provoke = false; // 도발
    public bool stealth = false; // 은신
    public bool stun = false; // 기절
    public bool silence = false; // 침묵
    public bool resistance = false; // 저항
    public bool blind = false; // 실명

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