using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour
{
    public Image iconSprite;
    public Button iconButton;

    public void SetSprite(string spriteKey)
    {
        iconSprite.sprite = GameManager.Instance.GetSpriteByAddress(spriteKey);
    }

    public void SetPortraitImage(Image refImage, string iconKey)
    {
        refImage.sprite = GameManager.Instance.GetSpriteByAddress(iconKey);
        GameManager.Instance.playerData.portraitKey = iconKey;
    }
}