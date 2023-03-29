using System;
using UnityEngine;

[SerializeField]
public class BufferState
{
    public float damage = 1;         // 공격력
    public float Damage => Math.Clamp(damage, -0.5f, 3f);

    public float defense = 1;      // 방어력
    public float damageReceived = 1;      // 받는 피해
    public float damageDecrease = 1;      // 주는 피해
    public float criticalProbability = 0;      // 치명타 확률 
    public float criticalDamage = 1;      // 치명타 피해량 증가
    public float attackSpeed = 1;      // 공격속도 증가
    public float maxHealthIncrease = 1;      // 최대 체력 증가

    public float freeze = 0;      // 빙결
    public bool isFreze = false;

    public float shield = 0;      // 보호막
    public bool isShield = false;

    public bool provoke = false; // 도발
    public bool stealth = false; // 은신
    public bool stun = false; // 기절
    public bool silence = false; // 침묵
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