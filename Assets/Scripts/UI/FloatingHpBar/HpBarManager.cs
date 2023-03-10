using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public struct HpInfo
{
    public HpInfo(float maxHp, float hp)
    {
        this.maxHp = maxHp;
        this.hp = hp;
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

    private HpInfo hpInfo;
    private Vector3 barLocation;
    private float maxHpDiv;

    public float barWidth = 100f;
    public float barHeigth = 20f;
    public float heightAboveGround = 1f;

    private bool isInstantiated = false;
    private bool isOn = false;
    private float timer = 0f;
    public float maxDuration = 3f;

    private float lastPosUpdateTime;
    private float posUpdateRatio;
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
        if(!hpBar.TryGetComponent(out canvasGroup))
            canvasGroup = hpBar.AddComponent<CanvasGroup>();
        
        posUpdateRatio = 1 / 20f;
    }
    void Update()
    {
        UpdateBar();
        CheckDurationTimer();
    }
    public void SetHp(float maxHp, float hp)
    {
        hpInfo = new HpInfo(maxHp, hp); 
        maxHpDiv = 1 / hpInfo.maxHp;
    }
    private void InstantiateHpBar()
    {
        if (isInstantiated)
            return;

        isInstantiated = true;
        hpBar = Instantiate(hpBar, canvasBarHolder.transform);
        hpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(barWidth, barHeigth);
    }
    private void UpdateBar()
    {
        UpdateBarPos();

        canvasGroup.alpha = isOn ? 1f : 0f;        

        hpBar.value = hpInfo.hp * maxHpDiv;
        //카메라 위치 체인지가 극심한 게임에선 필요함
        //if (hpBar.transform.position.z < 0f)
        //    canvasGroup.alpha = 0f;
    }
    private void UpdateBarPos()
    {
        if (Time.time - lastPosUpdateTime < posUpdateRatio)
            return;

        lastPosUpdateTime = Time.time;

        barLocation = transform.position;
        barLocation.y += heightAboveGround;
        hpBar.transform.position = cam.WorldToScreenPoint(barLocation);
    }

    //조건을 hp < 0 으로 걸어도됨. 대신 매프레임 체크나 외부에서 호출하는 식으로 바꿔야됨.
    public void Die()
    {
        hpBar.gameObject.SetActive(false);
        isOn = false;
    }
    private void CheckDurationTimer()
    {
        timer += Time.deltaTime;
        if (timer > maxDuration)
        {
            isOn = false;
        }
    }
    public void ActiveHpBar()
    {
        isOn = true;
        timer = 0f;
    }
    public void OnDamage(float damage)
    {
        hpInfo.hp -= damage;
        ActiveHpBar();
    }
}
