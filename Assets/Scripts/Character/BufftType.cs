public enum BuffType
{
    None = -1, // ����
    AttackIncrease,                 // ���ݷ� ����
    DefenseIncrease,                // ���� ����
    AttackDecrease,                 // ���ݷ� ����
    DefenseDecrease,                // ���� ����
    DamageIncrease,                 // �ִ� ���� ����
    DamageReceivedIncrease,         // �޴� ���� ����
    DamageDecrease,                 // �ִ� ���� ����
    DamageReceivedDecrease,         // �޴� ���� ����
    CriticalProbabilityIncrease,    // ġ��Ÿ Ȯ�� ����
    CriticalProbabilityDecrease,    // ġ��Ÿ Ȯ�� ����
    CriticalDamageIncrease,         // ġ��Ÿ ���ط� ����
    CriticalDamageDecrease,         // ġ��Ÿ ���ط� ����
    Provoke,                        // ����
    Stealth,                        // ����
    Stun,                           // ����
    Silence,                        // ħ��
    Resistance,                     // ����
    Blind,                          // �Ǹ�
    Count                           // ���� �̺�Ʈ�� ���� ��Ÿ���� ��
}