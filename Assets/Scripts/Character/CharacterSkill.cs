using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "CharacterSkill", menuName = "Character/CharacterSkill")]
public class CharacterSkill : ScriptableObject
{
    public LiveDataInBattle battleData;

    public int      id;
    public string   skillName;
    
    public float    preCooldown;
    public float    cooldown;       // ���� ���� ���� ��ٿ�
    public float    distance;       // ���� ���� (= �׺���̼� ���� �Ÿ�)
    public int      count;          // ��ų �ִ� ���� ��ü��
    public float    angle;          // ���� ���� ��. gameObject.transform.forward�� �������� �¿�� angle/2(��) ��ŭ ����

    // ���� �̿� �����, ������ ���� �� �ֱ� ������ �������� �����
    // �Ϲ� ��ų(��Ÿ)�� �ش� Ŭ���� ���ؼ� �ۼ�
    public SkillCoefficientType coefficientType; // ��ų ��� Ÿ��. ���ݷ�, ����, ü�� ���    

    public void TestDataInput(LiveData liveData)
    {
        battleData.Init(liveData);
    }
    public virtual void OnActive()
    {

    }
    public virtual IEnumerator SkillCoroutine()
    {
        yield break;  
    }
}