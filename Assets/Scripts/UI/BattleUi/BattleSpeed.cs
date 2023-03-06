using TMPro;
using UnityEngine;

public class BattleSpeed : MonoBehaviour
{
    public TextMeshProUGUI speedTxt;
    private float[] speeds = new float[3] { 1f, 2f, 3f };
    public float GetSpeed => speeds[idx];

    static int idx = 0;

    private void Awake()
    {
        idx = GameManager.Instance.playerData.battleSpeedIdx;
        SetBattleSpeed(idx);
    }

    public void SetBattleSpeed(int idx)
    {
        Time.timeScale = speeds[idx];
        speedTxt.text = $"{speeds[idx]:F1}x";
    }

    public void OnClickSpeedButton()
    {
        idx = (idx + 1) % speeds.Length;
        GameManager.Instance.playerData.battleSpeedIdx = idx;
        SetBattleSpeed(idx);
    }

    public void OnClickReturnButton()
    {
        Time.timeScale = speeds[idx];
        UIManager.Instance.ShowPanelInteractable(false);
    }
}
