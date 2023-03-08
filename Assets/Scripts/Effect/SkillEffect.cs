using UnityEngine;

public class SkillEffect : Effect
{
    public LiveData data;
    public CharacterSkill skill;
    public string targetTag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(targetTag))
        {
            int dmg = skill.CreateDamageResult(data);
            other.GetComponent<AttackableUnit>().OnDamage(dmg);
        }
    }
}
