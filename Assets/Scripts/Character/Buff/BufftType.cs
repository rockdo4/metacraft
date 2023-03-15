public enum BuffType
{
    None = 0, // 없음
    PowerUp = 1,
    DefenseUp = 2,
    PowerDown = 3,
    DefenseDown = 4,
    DamageReceivedUp,
    DamageDecreaseUp,
    DamageReceivedDown,
    DamageDecreaseDown,
    CriticalProbabilityUp = 9,
    CriticalProbabilityDown = 10,
    CriticalDamageUp,
    CriticalDamageDown,
    Provoke = 13,
    Stealth,
    Stun = 15,
    Silence = 16,
    Resistance,
    Blind,
    Burns,
    Freeze,
    AttackSpeedUp = 21,
    AttackSpeedDown = 22,
    MaxHealthIncrease = 23,
    Shield = 24,
    Bleed,
    Heal = 26,
    LifeSteal,
    energyCharging = 29,
    Count // 상태 이벤트의 수를 나타내는 값
}