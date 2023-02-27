using UnityEngine;

[CreateAssetMenu(fileName = "AttackColors", menuName = "Attack/UI/AttackColors")]
public class DamageTypeColors : ScriptableObject
{
    public Color[] colors = new Color[(int)DamageType.Count];    
}
