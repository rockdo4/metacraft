using UnityEngine;
using UnityEngine.UI;
public struct tempInfo
{
    public tempInfo(float maxHp)
    {
        this.maxHp = maxHp;
        this.hp = maxHp;
    }
    public float maxHp;
    public float hp;
}
public class HpBarManager : MonoBehaviour
{
    public Slider hpBar;
    private CanvasGroup canvasGroup;
    private CanvasHpBarHolder canvasBarHolder;
    private Camera cam;

    private tempInfo hpInfo = new tempInfo(100f);
    private Vector3 barLocation;
    private float maxHpDiv;

    public float barWidth = 100f;
    public float barHeigth = 20f;
    public float heightAboveGround = 1f;

    private bool isInstantiated = false;
    private bool isOn = false;
    private float prevHpValue;
    private float timer = 0f;
    public float maxDuration = 3f;

    private void OnEnable()
    {
        if (!isInstantiated)
            return;

        hpBar.gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        isOn = false;
    }
    void Start()
    {
        cam = Camera.main;
        canvasBarHolder = CanvasHpBarHolder.Instance;
        InstantiateHpBar();
        canvasGroup = hpBar.GetComponent<CanvasGroup>();
        prevHpValue = hpInfo.hp;
        maxHpDiv = 1 / hpInfo.maxHp;
    }
    void Update()
    {
        UpdateBar();
        CheckDurationTimer();
    }
    private void InstantiateHpBar()
    {
        if (isInstantiated)
            return;

        hpBar = Instantiate(hpBar, canvasBarHolder.transform);
        isInstantiated = true;
        hpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(barWidth, barHeigth);
    }
    private void UpdateBar()
    {
        barLocation = transform.position;
        barLocation.y += heightAboveGround;
        hpBar.transform.position = cam.WorldToScreenPoint(barLocation);

        canvasGroup.alpha = isOn ? 1f : 0f;

        hpBar.value = hpInfo.hp * maxHpDiv;
        //ī�޶� ��ġ ü������ �ؽ��� ���ӿ��� �ʿ���
        //if (hpBar.transform.position.z < 0f)
        //    canvasGroup.alpha = 0f;
    }
    //������ hp < 0 ���� �ɾ��. ��� �������� üũ�� �ٲ�ߵ�.
    public void WhenDieAnimationTriggered()
    {
        hpBar.gameObject.SetActive(false);
        isOn = false;
    }

    //�������� ������ �Ծ����� �˻��ϴ� �Լ�. �ʿ��ϸ� update���� ���.
    //private void CheckDamaged()
    //{
    //    var currHp = hpInfo.hp;
    //    if (currHp != prevHpValue)
    //    {
    //        isOn = true;
    //        timer = 0f;
    //    }
    //    prevHpValue = currHp;
    //}
    private void CheckDurationTimer()
    {
        timer += Time.deltaTime;
        if (timer > maxDuration)
        {
            isOn = false;
        }
    }
    public void WhenHit()
    {
        isOn = true;
        timer = 0f;
    }
    public void TestCode(float damage)
    {
        hpInfo.hp -= damage;
        WhenHit();
    }
}
