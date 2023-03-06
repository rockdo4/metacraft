public enum BuffType
{
    None = -1, // 없음
    AttackIncrease,                 // 공격력 증가
    DefenseIncrease,                // 방어력 증가
    AttackDecrease,                 // 공격력 감소
    DefenseDecrease,                // 방어력 감소
    DamageIncrease,                 // 주는 피해 증가
    DamageReceivedIncrease,         // 받는 피해 증가
    DamageDecrease,                 // 주는 피해 감소
    DamageReceivedDecrease,         // 받는 피해 감소
    CriticalProbabilityIncrease,    // 치명타 확률 증가
    CriticalProbabilityDecrease,    // 치명타 확률 감소
    CriticalDamageIncrease,         // 치명타 피해량 증가
    CriticalDamageDecrease,         // 치명타 피해량 감소
    Provoke,                        // 도발
    Stealth,                        // 은신
    Stun,                           // 기절
    Silence,                        // 침묵
    Resistance,                     // 저항
    Blind,                          // 실명
    Count                           // 상태 이벤트의 수를 나타내는 값
}