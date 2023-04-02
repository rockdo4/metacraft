using UnityEngine;

public class SaintessActive : MonoBehaviour
{
    public AttackableUnit unit;

    public ParticleSystem particle;

    public void OnActiveSkill()
    {
        var heros = unit.HeroList;
        
        foreach(var hero in heros)
        {
            var effect = Instantiate(particle, hero.transform);
            effect.transform.localScale = Vector3.one / 2.5f;
            Destroy(effect.gameObject, 5);
        }
    }
}
