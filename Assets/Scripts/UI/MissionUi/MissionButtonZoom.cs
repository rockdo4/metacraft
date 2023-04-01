using System.Collections;
using UnityEngine;

public class MissionButtonZoom : MonoBehaviour
{
    public RectTransform mapImageRectTransform;
    public float zoomAmount = 2f;
    public float zoomDuration = 1f;
    private float zoomDurationInverse;
    private Vector2 originalPivot;
    private Vector2 originalAnchoredPosition;
    private Vector3 originalScale;
    public MissionMarkData[] marks;

    private void Start()
    {
        originalPivot = mapImageRectTransform.pivot;
        originalAnchoredPosition = mapImageRectTransform.anchoredPosition;
        originalScale = mapImageRectTransform.localScale;
        zoomDurationInverse = 1 / zoomDuration;
    }

    public void ZoomInButtonClicked(Vector3 pos)
    {
        StartCoroutine(ZoomInCoroutine(pos));
    }

    //private void OnDisable()
    //{
    //    ZoomOutImmediatelyButtonClicked();
    //}

    private IEnumerator ZoomInCoroutine(Vector3 pos)
    {
        Vector3 targetPivot = Camera.main.ScreenToViewportPoint(pos);
        Vector3 targetScale = originalScale * zoomAmount;
        Vector3 targetAnchoredPosition = new Vector2(0, 0);

        foreach (var mark in marks)
        {
            if (mark.isMarkOn)
            {
                mark.gameObject.SetActive(false);
            }
        }

        float elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            float t = elapsedTime * zoomDurationInverse; // / zoomDuration;
            mapImageRectTransform.pivot = Vector3.Lerp(originalPivot, targetPivot, t);
            mapImageRectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            mapImageRectTransform.anchoredPosition = Vector2.Lerp(originalAnchoredPosition, targetAnchoredPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mapImageRectTransform.pivot = targetPivot;
        mapImageRectTransform.localScale = targetScale;
        mapImageRectTransform.anchoredPosition = targetAnchoredPosition;

    }

    public void ZoomOutButtonClicked()
    {
        StartCoroutine(ZoomOutCoroutine());
    }

    private IEnumerator ZoomOutCoroutine()
    {
        Vector3 targetPivot = mapImageRectTransform.pivot;
        Vector3 targetScale = mapImageRectTransform.localScale;
        Vector3 targetAnchoredPosition = mapImageRectTransform.anchoredPosition;

        foreach (var mark in marks)
        {
            if (mark.isMarkOn)
            {
                mark.gameObject.SetActive(true);
            }
        }

        float elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            float t = elapsedTime * zoomDurationInverse; // / zoomDuration;
            mapImageRectTransform.pivot = Vector3.Lerp(targetPivot, originalPivot, t);
            mapImageRectTransform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            mapImageRectTransform.anchoredPosition = Vector2.Lerp(targetAnchoredPosition, originalAnchoredPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mapImageRectTransform.pivot = originalPivot;
        mapImageRectTransform.localScale = originalScale;
        mapImageRectTransform.anchoredPosition = originalAnchoredPosition;
    }

    public void ZoomOutImmediatelyButtonClicked()
    {
        //Vector3 targetPivot = mapImageRectTransform.pivot;
        //Vector3 targetScale = mapImageRectTransform.localScale;
        //Vector3 targetAnchoredPosition = mapImageRectTransform.anchoredPosition;

        foreach (var mark in marks)
        {
            if (mark.isMarkOn)
            {
                mark.gameObject.SetActive(true);
            }
        }
        mapImageRectTransform.pivot = originalPivot;
        mapImageRectTransform.localScale = originalScale;
        mapImageRectTransform.anchoredPosition = originalAnchoredPosition;
    }
}