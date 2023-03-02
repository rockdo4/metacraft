using UnityEngine;

public class BattleUi : View
{
    public Transform herosTr;
    public StageEnemy stageEnemy;
    public BattleMenu battleMenu;
    public Transform popUp;
    private TestBattleManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<TestBattleManager>();
    }

    public void SetEnemyCount(int c) => stageEnemy.Count = c;

    public void OnClickMenu()
    {
        Instantiate(battleMenu, transform);
    }
    public void OnPause()
    {
        Time.timeScale = 0f;
    }
    public void OffPause()
    {
        Time.timeScale = 1f;
    }
    public void ResetGame()
    {
        manager.ResetHeroes();
        GameManager.Instance.ClearBattleGroups();
    }
}
