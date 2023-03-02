using TMPro;
using UnityEngine;

public class BattleSpeed : MonoBehaviour
{
    public TextMeshProUGUI speedTxt;
    float[] speeds = new float[3] { 1f, 2f, 3f };
    string speedFormat = "{0:F1}x";

    int idx = 0;

    private void Awake()
    {
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
        SetBattleSpeed(idx);
    }
}
