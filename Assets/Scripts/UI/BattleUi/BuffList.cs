using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffList : MonoBehaviour
{
    // private readonly int maxViewBuffCount = 3;

    public BuffIcon buffPref;
    public PopUpBuffIcon buffPopUpPref;
    [SerializeField]
    public List<BuffIcon> uiBuffList = new(); //���� ����� ���� ����Ʈ

    [SerializeField]
    private Transform viewBuffTr; //ĳ���� ���� ����
    [SerializeField]
    private Transform popUpBuffTr; //�˾�â ����
    [SerializeField]
    public GameObject popUp;

    public Image plusImage;
    public void BuffButtonCheck()
    {
        if(uiBuffList.Count <= 3)
        {
            plusImage.gameObject.SetActive(false);
        }
    }
    public BuffIcon AddIcon(BuffType type, float duration, int idx, Sprite sprite)
    {
        var viewBuff = Instantiate(buffPref, viewBuffTr);   //ĳ���� ������ ����Ʈ�� ���� �߰��ϰ�, �˾����� �߰�
        var popUpBuff = Instantiate(buffPopUpPref, popUpBuffTr);

        viewBuff.transform.SetSiblingIndex(idx);
        popUpBuff.transform.SetSiblingIndex(idx);
        viewBuff.iconImage.sprite = sprite;
        popUpBuff.iconImage.sprite = sprite;

        viewBuff.buffType = type;

        viewBuff.popUpBuff = popUpBuff;
        uiBuffList.Add(viewBuff); 

        viewBuff.OnEnd += () => uiBuffList.Remove(viewBuff);
        viewBuff.OnEnd += BuffButtonCheck;

        if (uiBuffList.Count > 3)
        {
            plusImage.gameObject.SetActive(true);
        }

        return viewBuff;
    }

    public void RemoveBuff(BuffIcon buffIcon)
    {
        buffIcon.EndBuffIcon();
        uiBuffList.Remove(buffIcon);
    }
    public void OnClickPopUp()
    {
        if(!popUp.activeSelf)
            popUp.SetActive(true);
    }
}
