using System.Collections.Generic;

public class BufferState
{
    public float attackIncrease                 = 1;
    public float defenseIncrease                = 1; 
    public float attackDecrease                 = 1;
    public float defenseDecrease                = 1; 
    public float damageIncrease                 = 1;
    public float damageReceivedIncrease         = 1;
    public float damageDecrease                 = 1;
    public float damageReceivedDecrease         = 1; 
    public float criticalProbabilityIncrease    = 1;
    public float criticalProbabilityDecrease    = 1; 
    public float criticalDamageIncrease         = 1; 
    public float criticalDamageDecrease         = 1; 

    public bool provoke     = false;
    public bool stealth     = false;
    public bool stun        = false;
    public bool silence     = false;
    public bool resistance  = false;
    public bool blind       = false;

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