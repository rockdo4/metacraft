using System.Collections;
using UnityEngine;

public class AP_CreateProjectiles : MonoBehaviour
{
    public Rigidbody Projectile;
    public GameObject Launch;
    public float Time = 1.0f;


    private bool CreateInstances = true;
    private Rigidbody Instance;
    private GameObject Explosion;
    private ParticleSystem Cast;

    void Awake()
    {
        Cast = GetComponent<ParticleSystem>();
        Cast.Stop();
    }

    private void OnEnable()
    {
        Cast.Stop();

        if (Instance)
        {
            Destroy(Instance);
        }

        if (Explosion)
        {
            Destroy(Explosion);
        }
        CreateInstances = true;
        StopCoroutine("ToCreate");
        StopCoroutine("Create");
        ToCreate();
    }

    void Update()
    {
        if (Instance == null && Explosion == null)
        {
            CreateInstances = true;
            ToCreate();

        }

    }
    void ToCreate()
    {
        StartCoroutine("Create");
    }
    public IEnumerator Create()
    {
        if (Cast.isStopped && CreateInstances)
        {
            Cast.Play();
            yield return new WaitForSeconds(Time);

            GameObject LaunchInstance;
            LaunchInstance = Instantiate(Launch, transform.position, Launch.transform.rotation);
            Destroy(LaunchInstance, 2.0f);

            Instance = Instantiate(Projectile, transform.position, Projectile.transform.rotation);
            CreateInstances = false;
            Instance.transform.SetParent(this.transform);
        }
    }


    public void GetExplosion(GameObject Exp)
    {
        Explosion = Exp;
    }
}
