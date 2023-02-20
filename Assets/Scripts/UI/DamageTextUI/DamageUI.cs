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
    public float yPosUp = 1f;
    
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
        pos.y += transSpeed * Time.deltaTime;
        transform.position = cam.WorldToScreenPoint(pos);
        //뷰가 심하게 돌아가는 게임에선 아래 코드 사용해야함
        //textMeshPro.alpha = transform.position.z > 0f ? 1f : 0f;
    }
    public void SetUI(int damage, Vector3 enemyPos)
    {
        textMeshPro.text = damage.ToString();
        pos = enemyPos + Vector3.up * yPosUp;        
        transform.position = cam.WorldToScreenPoint(pos);
        Invoke("Realese", lifeTime);
    }
    private void Realese()
    {
        pool.Release(this);
    }     
}
