using System.Collections.Generic;

public class BufferState
{
    public float attackIncrease                 = 1;    // 공격력 증가
    public float defenseIncrease                = 1;    // 방어력 증가
    public float attackDecrease                 = 1;    // 공격력 감소
    public float defenseDecrease                = 1;    // 방어력 감소
    public float damageIncrease                 = 1;    // 주는 피해 증가
    public float damageReceivedIncrease         = 1;    // 받는 피해 증가
    public float damageDecrease                 = 1;    // 주는 피해 감소
    public float damageReceivedDecrease         = 1;    // 받는 피해 감소
    public float criticalProbabilityIncrease    = 1;    // 치명타 확률 증가
    public float criticalProbabilityDecrease    = 1;    // 치명타 확률 감소
    public float criticalDamageIncrease         = 1;    // 치명타 피해량 증가
    public float criticalDamageDecrease         = 1;    // 치명타 피해량 감소
                                                        
    public bool provoke     = false;                    // 도발
    public bool stealth     = false;                    // 은신
    public bool stun        = false;                    // 기절
    public bool silence     = false;                    // 침묵
    public bool resistance  = false;                    // 저항
    public bool blind       = false;                    // 실명

    public void Buffer(BuffType type, float scale)
    {
        switch (type)
        {
            case BuffType.AttackIncrease:
                attackIncrease += scale;
                break;
            case BuffType.DefenseIncrease:
                defenseIncrease += scale;
                break;
            case BuffType.AttackDecrease:
                attackDecrease += scale;
                break;
            case BuffType.DefenseDecrease:
                defenseDecrease += scale;
                break;
            case BuffType.DamageIncrease:
                damageIncrease += scale;
                break;
            case BuffType.DamageReceivedIncrease:
                damageReceivedIncrease += scale;
                break;
            case BuffType.DamageDecrease:
                damageDecrease += scale;
                break;
            case BuffType.DamageReceivedDecrease:
                damageReceivedDecrease += scale;
                break;
            case BuffType.CriticalProbabilityIncrease:
                criticalProbabilityIncrease += scale;
                break;
            case BuffType.CriticalProbabilityDecrease:
                criticalProbabilityDecrease += scale;
                break;
            case BuffType.CriticalDamageIncrease:
                criticalDamageIncrease += scale;
                break;
            case BuffType.CriticalDamageDecrease:
                criticalDamageDecrease += scale;
                break;
            default:
                break;
        }
    }
}