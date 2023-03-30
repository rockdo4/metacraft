using UnityEngine;

[CreateAssetMenu(fileName = "TypeColor", menuName = "Character/TypeColor")]
public class TypeColor : ScriptableObject
{
    [SerializeField]
    private Color assultColor;
    [SerializeField]
    private Color defenceColor;
    [SerializeField]
    private Color shooterColor;
    [SerializeField]
    private Color assassinColor;
    [SerializeField]
    private Color assistColor;
    public Color GetTypeColor(CharacterJob job)
    {
        switch (job)
        {
            case CharacterJob.assult:
                return assultColor;
            case CharacterJob.defence:
                return defenceColor;
            case CharacterJob.shooter:
                return shooterColor;
            case CharacterJob.assist:
                return assistColor;
        }
        return Color.black;
    }
}
