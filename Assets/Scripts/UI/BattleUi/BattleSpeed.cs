using TMPro;
using UnityEngine;

public class BattleSpeed : MonoBehaviour
{
    public TextMeshProUGUI speedTxt;
    float[] speeds = new float[3] { 1f, 2f, 3f };
    string speedFormat = "{0:F1}x";
    public float GetSpeed => speeds[idx];
    int idx = 0;

    private void Awake()
    {
        idx = GameManager.Instance.playerData.battleSpeedIdx;
        SetBattleSpeed(idx);
    }
    public void SetBattleSpeed(int idx)
    {
        Time.timeScale = speeds[idx];
        speedTxt.text = string.Format(speedFormat, speeds[idx]);
    }

    public void OnClickSpeedButton()
    {
        idx = (idx + 1) % speeds.Length;
        GameManager.Instance.playerData.battleSpeedIdx = idx;
        SetBattleSpeed(idx);
    }
    public void OnClickReturnButton()
    {
        Time.timeScale = speeds[GameManager.Instance.playerData.battleSpeedIdx];
        UIManager.Instance.ShowPanelInteractable(false);
    }
}