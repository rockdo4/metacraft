using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class DamageUI : MonoBehaviour
{
    private IObjectPool<DamageUI> pool;    
    private TextMeshProUGUI textMeshPro;
    private Camera cam;
    private Vector3 pos;

    public float transSpeed = 1f;
    public float lifeTime = 2f;    
    public DamageTypeColors damageTypeColors;    

    private string releaseMethodName = nameof(Realese);

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();        
        cam = Camera.main;        
    }   
    public void SetPool(IObjectPool<DamageUI> pool)
    {
        this.pool = pool;        
    }
    private void Update()
    {
        transform.Translate(0f, transSpeed * Time.deltaTime, 0f);
        //�䰡 ���ϰ� ���ư��� ���ӿ��� �Ʒ� �ڵ� ����ؾ���
        //textMeshPro.alpha = transform.position.z > 0f ? 1f : 0f;
    }
    public void SetUI(int damage, Vector3 enemyPos, DamageType type, float heightAboveGround)
    {
        textMeshPro.text = damage.ToString();
        textMeshPro.color = damageTypeColors.colors[(int)type];
        pos = enemyPos + Vector3.up * heightAboveGround;        
        transform.position = cam.WorldToScreenPoint(pos);        
        Invoke(releaseMethodName, lifeTime);  
    }
    private void Realese()
    {
        pool.Release(this);
    }     
}