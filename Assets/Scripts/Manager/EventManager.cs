using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

// ���� ������ ������ ��������
// ��Ʈ��ũ���̸� ���� ��Ʈ��ũ�Ѵ�� ����ϰ�
// ������� ���� ������� ����ϵ�
// ui�� ����ؼ� �̺�Ʈ�� ������ ����

// ������ �ʵ�
// ���� ��Ʈ��ũ��
// ���� �����
// Ʈ���� 1ĭ�ִ� ��Ʈ��ũ�� (���� �����ε� ����� ����)
// �渷 ������Ʈ �ִ� ��
// ���... (���� ���� �ȳ�)

public class EventManager : MonoBehaviour
{
    [Header("???? ??? ?? ????? ???? ????????.")]
    public List<GameObject> eventMaps;
    private List<Dictionary<string, object>> eventInfoTable;

    public MapEventEnum curEvent = MapEventEnum.Normal;
    private GameObject curMap;

    public GameObject eventUi;
    [Header("?????? ?????")]
    public List<GameObject> choiceButtons;
    [Header("????? ??????? ??? Ui")]
    public Image heroImage;
    [Header("???? ?????? ??? ???? ???????")]
    public TextMeshProUGUI contentText;

    private List<TextMeshProUGUI> buttonTexts = new(); // ????? ?????? ??????

    private List<string> heroNames = new();

    public void Awake()
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            var text = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonTexts.Add(text);
        }

        GameManager gm = GameManager.Instance;

        eventInfoTable = gm.eventInfoList;
        // ???? ???? ????????? ???????? ??
        // ????) eventInfoTable[0]["Explanation"]

        var useHeroes = gm.GetSelectedHeroes();
        for (int i = 0; i < useHeroes.Count; i++)
        {
            heroNames.Add(useHeroes[i].name);
        }

        StartEvent(curEvent);
    }

    public void StartEvent(MapEventEnum ev) // ???? ????
    {
        // ???? ??????? ??? ?? ???? ?? curMap?? ?????? ?? ????????
        // SetActiveEventMap ?????? ??? ?? true?????
        curEvent = ev;

        // ???? ui ?? ?? ???
        SetEventUiProperty(curEvent);
    }

    public void EndEvent() // ???? ???? ui ?????, ?? ??? ?????? ??? ?????
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        SetEventUiActive(false);
    }

    public void SetActiveEventMap(bool active) // ???? ???? ??? ?? ????? ????
    {
        curMap.SetActive(active);
    }

    public void SetEventUiProperty(MapEventEnum ev) // ?????? ???? ????
    {
        if (curMap != null)
            SetActiveEventMap(false);

        if (ev == MapEventEnum.Normal)
        {
            curMap = eventMaps[0];
        }
        else if (ev == MapEventEnum.Defense)
        {
            curMap = eventMaps[1];
        }
        else
        {
            curMap = eventMaps[2];
            SetEventInfo(ev);
            SetEventUiActive(true);
        }

        SetActiveEventMap(true);
    }

    public void SetEventUiActive(bool active) => eventUi.SetActive(active);

    public void SetEventInfo(MapEventEnum ev)
    {
        int heroNameIndex = Random.Range(0, heroNames.Count);
        heroImage.sprite = GameManager.Instance.GetSpriteByAddress($"Icon_{heroNames[heroNameIndex]}");
        contentText.text = $"{eventInfoTable[(int)ev]["Explanation"]}";

        int textCount = (int)eventInfoTable[(int)ev][$"TextCount"];
        Logger.Debug(textCount);
        for (int i = 0; i < textCount; i++)
        {
            choiceButtons[i].SetActive(true);
            string text = $"{eventInfoTable[(int)ev][$"Select{i + 1}Text"]}";
            buttonTexts[i].text = text;
        }
    }
}
