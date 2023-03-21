using UnityEngine;

public class Recovery : MonoBehaviour
{
    public CharacterSkill skill;
    
    public float interval = 5f;
    
    private float lastRecoverTime;
    private AttackableUnit unit;
    

    private void Awake()
    {
        unit = GetComponent<AttackableUnit>();        
    }
    private void Update()
    {
        if (Time.time - lastRecoverTime < interval)
            return; 
        
        lastRecoverTime = Time.time;
        unit.OnDamage(unit, skill);
    }
}
