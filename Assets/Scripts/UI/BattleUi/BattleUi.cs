using UnityEngine;

public class BattleUi : View
{
    public Transform herosTr;
    public StageEnemy stageEnemy;
    public BattleMenu battleMenu;
    public Transform popUp;
    //private bool mapFlag = false;

    public void SetEnemyCount(int c) => stageEnemy.Count = c;

    public void OnClickMenu()
    {
        Instantiate(battleMenu, transform);
    }
    public void OnPause()
    {
        Logger.Debug("Pause");
        Time.timeScale = 0f;
    }
    public void OffPause()
    {
        Time.timeScale = 1f;
    }
    public void ResetGame()
    {
        GameManager.Instance.ClearBattleGroups();
        //mapFlag = false;
    }
}
