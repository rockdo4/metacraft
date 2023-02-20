using UnityEngine;
using UnityEngine.Pool;
public class DamageUiObjPool : MonoBehaviour
{
    static private DamageUiObjPool instance;
    static public DamageUiObjPool Instance { get { return instance; } }


    public DamageUI normalPrefab;
    public DamageUI critPrefab;
    public int normalMaxSize = 256;
    public int critMaxSize = 256;

    public IObjectPool<DamageUI> NormalPool { get; private set; }
    public IObjectPool<DamageUI> CritPool { get; private set; }
    private void Awake()
    {
        instance = this;

        NormalPool = new ObjectPool<DamageUI>(
            CreateNormal,
            OnGet,
            OnRealese,
            OnDestroyUI,
            maxSize : normalMaxSize
            );
        CritPool = new ObjectPool<DamageUI>(
            CreateCrit,
            OnGet,
            OnRealese,
            OnDestroyUI,
            maxSize: critMaxSize
            );    
    }
    private DamageUI CreateNormal()
    {
        DamageUI normalUi = Instantiate(normalPrefab, transform);
        normalUi.SetPool(NormalPool);
        return normalUi;
    }
    private DamageUI CreateCrit()
    {
        DamageUI critUi = Instantiate(critPrefab, transform);
        critUi.SetPool(CritPool);
        return critUi;
    }
    private void OnGet(DamageUI ui)
    {
        ui.gameObject.SetActive(true);
    }
    private void OnRealese(DamageUI ui)
    {
        ui.gameObject.SetActive(false);
    }
    private void OnDestroyUI(DamageUI ui)
    {
        Destroy(ui.gameObject);
    }
}
