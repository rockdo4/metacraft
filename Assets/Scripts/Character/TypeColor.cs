using UnityEngine;

[CreateAssetMenu(fileName = "TypeColor", menuName = "Character/TypeColor")]
public class TypeColor : ScriptableObject
{
    [SerializeField]
    private Color assultColor = Color.green;
    [SerializeField]
    private Color defenceColor = new Color32(143, 0, 254, 255);
    [SerializeField]
    private Color shooterColor = Color.red;
    [SerializeField]
    private Color assassinColor = Color.gray;
    [SerializeField]
    private Color assistColor = Color.cyan;
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
