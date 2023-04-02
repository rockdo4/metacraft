using UnityEngine;

public class SaintessActive : MonoBehaviour
{
    public AttackableUnit unit;

    public void OnActiveSkill()
    {
        var heros = unit.HeroList;
        
        foreach(var hero in heros)
        {
            
        }
    }
}
