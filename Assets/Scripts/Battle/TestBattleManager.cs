using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TestBattleManager : MonoBehaviour
{
    public GameObject heroList;
    public List<HeroUi> heroUiList;
    public List<Transform> startPositions;
    public List<AttackableHero> useHeroes = new();
    public StageEnemy enemyCountTxt;

    public ClearUiController clearUi;
    public List<MapEventTrigger> triggers;

    protected int readyCount;

    public Image fadePanel;

    public GeneratePolynomialTreeMap tree;
    public TreeNodeObject thisNode;

    // Test Member
    public List<ForkedRoad> enableRoads = new();
    public List<ForkedRoad> disableRoads = new();
    public ForkedRoad roadPrefab;
    public Transform roadTr;
    public List<Vector3> roadRots = new List<Vector3> { new (0,-45,0), new (0,45,0), new(0, 0, 0) };
    public int roadCount = 3;
    protected Coroutine coFadeIn;
    protected Coroutine coFadeOut;
    public List<RoadChoiceButton> choiceButtons;
    private List<TextMeshProUGUI> choiceButtonTexts = new();
    protected int nodeIndex;

    private void Awake()
    {
        List<GameObject> selectedHeroes = GameManager.Instance.GetSelectedHeroes();
        int count = selectedHeroes.Count;

        for (int i = 0; i < count; i++)
        {
            if (selectedHeroes[i] != null)
            {
                selectedHeroes[i].SetActive(true);
                Utils.CopyPositionAndRotation(selectedHeroes[i], startPositions[i]);
                NavMeshAgent heroNav = selectedHeroes[i].GetComponent<NavMeshAgent>();
                heroNav.enabled = true;
                AttackableHero attackableHero = selectedHeroes[i].GetComponent<AttackableHero>();
                attackableHero.SetBattleManager(this);
                attackableHero.SetUi(heroUiList[i]);
                attackableHero.ResetData();
                heroUiList[i].SetHeroInfo(attackableHero.GetUnitData());
                heroUiList[i].gameObject.SetActive(true);
                useHeroes.Add(attackableHero);
            }
        }
        
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var text = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            choiceButtonTexts.Add(text);
        }

        tree.CreateTreeGraph();
        thisNode = tree.root; // ���� ��ġ�� ���

        // tree.root.type �� Ÿ��
        // tree.root.childrens �� ����
        // thisNode = tree.root.childrens[0]; ���� ��� ������ �� ���� ��

        clearUi.SetHeroes(useHeroes);
        readyCount = useHeroes.Count;
    }

    public List<Transform> GetStartPosition()
    {
        return startPositions;
    }

    public int GetAllEnemyCount()
    {
        int count = 0;
        for (int i = 0; i < triggers.Count; i++)
        {
            count += triggers[i].enemys.Count;
        }

        return count;
    }

    public void GetHeroList(ref List<AttackableHero> heroList)
    {
        heroList = useHeroes;
    }

    public virtual void OnDeadHero(AttackableHero hero)
    {
        useHeroes.Remove(hero);
        readyCount = useHeroes.Count;
        if (useHeroes.Count == 0)
        {
            SetStageFail();
        }
    }
    public virtual void OnDeadEnemy(AttackableEnemy enemy)
    {
        enemyCountTxt.DieEnemy();
    }
    public virtual void GetEnemyList(ref List<AttackableEnemy> enemyList) { }
    public virtual void OnReady()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.MoveNext);
        }
    }
    public void EnemyTriggerEnter()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            useHeroes[i].ChangeUnitState(UnitState.Battle);
        }
    }
    protected virtual void SetHeroReturnPositioning(List<Transform> pos)
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Logger.Debug("Return");
            useHeroes[i].SetReturnPos(pos[i]);
            useHeroes[i].ChangeUnitState(UnitState.ReturnPosition);
        }
    }
    // Ŭ���� �� ȣ���� �Լ� (Ui ������Ʈ)
    protected void SetStageClear()
    {
        UIManager.Instance.ShowView(1);
        GameManager.Instance.NextDay();
        clearUi.SetData();
        Logger.Debug("Clear!");
    }

    protected IEnumerator CoFadeIn()
    {
        fadePanel.gameObject.SetActive(true);
        float fadeAlpha = 0f;
        while (fadeAlpha < 1f)
        {
            fadeAlpha += 0.01f;
            yield return null;
            fadePanel.color = new Color(0, 0, 0, fadeAlpha);
        }
        yield break;
    }

    protected IEnumerator CoFadeOut()
    {
        float fadeAlpha = 1f;
        while (fadeAlpha > 0f)
        {
            fadeAlpha -= 0.01f;
            yield return null;
            fadePanel.color = new Color(0, 0, 0, fadeAlpha);
        }

        fadePanel.gameObject.SetActive(false);
        yield break;
    }

    public virtual void SelectNextStage(int index)
    {
        int stageIndex = nodeIndex = choiceButtons[index].choiceIndex;
        thisNode = thisNode.childrens[stageIndex];

        readyCount = useHeroes.Count;

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }

        // tree.root.type �� Ÿ��
        // tree.root.childrens �� ����
        // thisNode = tree.root.childrens[0]; ���� ��� ������ �� ���� ��
    }

    protected void ChoiceNextStage()
    {
        for (int i = 0; i < thisNode.childrens.Count; i++)
        {
            choiceButtonTexts[i].text = $"{thisNode.childrens[i].type}";
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].choiceIndex = i;
        }
    }

    public virtual void MoveNextStage(float timer)
    {
        coFadeIn = StartCoroutine(CoFadeIn());
    }

    // ����ε� �� ���̴� ��ġ�� �ű�� Active False ��Ű�� �Լ�
    public void ResetHeroes()
    {
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Utils.CopyPositionAndRotation(useHeroes[i].gameObject, GameManager.Instance.heroSpawnTransform);
            useHeroes[i].SetEnabledPathFind(false);
            useHeroes[i].gameObject.SetActive(false);
        }
    }

    public void OnDestroy()
    {
        Time.timeScale = 1;
    }

    protected void SetStageFail()
    {
        Time.timeScale = 0;
        GameManager.Instance.NextDay();
        UIManager.Instance.ShowView(2);
        Logger.Debug("Fail!");
    }

    // ��� ����
    protected void CreateRoad(GameObject platform)
    {
        for (int i = 0; i < roadCount; i++)
        {
            ForkedRoad road = Instantiate(roadPrefab, platform.transform);
            Transform tr = roadTr;
            tr.transform.localRotation = Quaternion.Euler(roadRots[i]);
            road.SetRoadChangeAngle(tr);
            road.gameObject.SetActive(false);
            disableRoads.Add(road);
        }
    }

    protected void SetRoad()
    {
        Logger.Debug(thisNode.childrens.Count);

        for (int i = 0; i < thisNode.childrens.Count; i++)
        {
            enableRoads[i].gameObject.SetActive(true);
        }
    }

    protected void EnableRoad()
    {
        for (int i = 0; i < thisNode.childrens.Count; i++)
        {
            enableRoads.Add(disableRoads[i]);
            enableRoads[i].gameObject.SetActive(true);
            triggers.Add(enableRoads[i].fadeTrigger);
        }
    }

    protected void DisableRoad()
    {
        for (int i = 0; i < enableRoads.Count; i++)
        {
            triggers.Remove(enableRoads[i].fadeTrigger);
        }

        enableRoads.Clear();
    }

    protected void ResetStage()
    {
        for (int i = 0; i < triggers.Count; i++)
        {
            triggers[i].ResetEnemys();
        }
        for (int i = 0; i < useHeroes.Count; i++)
        {
            Utils.CopyPositionAndRotation(useHeroes[i].gameObject, startPositions[i]);
        }
    }
}
