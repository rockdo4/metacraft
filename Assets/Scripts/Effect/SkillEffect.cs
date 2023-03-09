using UnityEngine;

public class SkillEffect : Effect
{
    public LiveData liveData;
    public CharacterSkill skill;
    public string targetTag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(targetTag))
        {
            int dmg = skill.CreateDamageResult(liveData);
            other.GetComponent<AttackableUnit>().OnDamage(dmg);
        }
    }
}
