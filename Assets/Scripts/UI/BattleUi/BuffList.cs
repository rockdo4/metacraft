using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffList : MonoBehaviour
{
    // private readonly int maxViewBuffCount = 3;

    public HeroBuff buffPref;
    public GameObject buffPopUpPref;
    [SerializeField]
    public List<HeroBuff> buffList; //���� ����� ���� ����Ʈ

    [SerializeField]
    private Transform viewBuffTr; //ĳ���� ���� ����
    [SerializeField]
    private Transform popUpBuffTr; //�˾�â ����
    [SerializeField]
    public GameObject popUp;

    public Image plusImage;

    public void SetList(ref List<HeroBuff> heroBuffs) => this.buffList = heroBuffs;

    public void DelBuff()
    {
        if(buffList.Count <= 3)
        {
            plusImage.gameObject.SetActive(false);
        }
    }
    public void AddBuff()
    {
        var viewBuff = Instantiate(buffPref, viewBuffTr);   //ĳ���� ������ ����Ʈ�� ���� �߰��ϰ�, �˾����� �߰�
        var popUpBuff = Instantiate(buffPopUpPref, popUpBuffTr);

        viewBuff.popUpBuff = popUpBuff;
        viewBuff.duration = 3f;

        buffList.Add(viewBuff); 

        viewBuff.OnEnd += () => buffList.Remove(viewBuff);
        viewBuff.OnEnd += DelBuff;


        if (buffList.Count >= 3)
        {
            plusImage.gameObject.SetActive(true);
        }
    }

    public void OnClickPopUp()
    {
        if(!popUp.activeSelf)
            popUp.SetActive(true);
    }
}
