using UnityEngine;

[CreateAssetMenu(fileName = "AttackColors", menuName = "Attack/AttackColors")]
public class DamageTypeColors : ScriptableObject
{
    public Color[] colors = new Color[(int)DamageType.Count];    
}
