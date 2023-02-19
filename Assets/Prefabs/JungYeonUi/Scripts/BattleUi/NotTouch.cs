using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotTouch : MonoBehaviour
{
    public RectTransform rectTr;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ��ġ ������ ��ǥ�� �����ɴϴ�.
            Vector2 touchPos = Input.mousePosition;

            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTr, touchPos))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
