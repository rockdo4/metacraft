using UnityEngine;

public class ZoomInOutClick : MonoBehaviour
{
    public MissionButtonZoom missionButtonZoom;

    public void ZoomInButtonClicked()
    {
        missionButtonZoom.ZoomInButtonClicked(transform.position);
    }

    public void ZoomOutButtonClicked()
    {
        missionButtonZoom.ZoomOutButtonClicked();
    }
}
