using UnityEngine;

public class LiveDataInBattle : MonoBehaviour
{
    public int baseDamage;
    public int baseDefense;
    public int healthPoint;  
    public int moveSpeed;    
    public float critical;   
    public float criticalDmg;
    public float evasion;    
    public float accuracy;
    public void Init(LiveData liveData)
    {
        baseDamage  = liveData.baseDamage;
        baseDefense = liveData.baseDefense;
        healthPoint = liveData.healthPoint;
        moveSpeed   = liveData.moveSpeed;
        critical    = liveData.critical;
        criticalDmg = liveData.criticalDmg;
        evasion     = liveData.evasion;
        accuracy    = liveData.accuracy;
    }
}
