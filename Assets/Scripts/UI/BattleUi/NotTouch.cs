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
            // 터치 지점의 좌표를 가져옵니다.
            Vector2 touchPos = Input.mousePosition;

            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTr, touchPos))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
