
public class BufferState
{
    public float power = 0;         // 공격력
    public float defense = 0;      // 방어력
    public float damageReceived = 0;      // 받는 피해
    public float damageDecrease = 0;      // 주는 피해
    public float criticalProbability = 0;      // 치명타 확률 
    public float criticalDamage = 0;      // 치명타 피해량 증가
    public float attackSpeed = 0;      // 공격속도 증가
    public float maxHealthIncrease = 0;      // 최대 체력 증가

    public float burns = 0;      // 화상
    public bool isBurns = false;

    public float freeze = 0;      // 빙결
    public bool isFreze = false;

    public float shield = 0;      // 보호막
    public bool isShield = false;

    public float bleed = 0;      // 출혈
    public bool isBleed = false;

    public float healthRegen = 0;      // 체력 회복
    public bool isHealtRegen = false;

    public float lifeSteal = 0;      // 흡혈
    public bool isLifeSteal = false;

    public bool provoke = false; // 도발
    public bool stealth = false; // 은신
    public bool stun = false; // 기절
    public bool silence = false; // 침묵
    public bool resistance = false; // 저항
    public bool blind = false; // 실명

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