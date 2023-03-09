using UnityEngine;

public class BattleUi : View
{
    public Transform herosTr;
    public StageEnemy stageEnemy;
    public BattleMenu battleMenu;
    public Transform popUp;
    private TestBattleManager battleManager;
    private bool mapFlag = false;

    private void Awake()
    {
        battleManager = FindObjectOfType<TestBattleManager>();
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
        battleManager.ResetHeroes();
        GameManager.Instance.ClearBattleGroups();
        mapFlag = false;
    }

    public void SwitchMap()
    {
        mapFlag = !mapFlag;
        battleManager.tree.gameObject.SetActive(mapFlag);
    }
}
