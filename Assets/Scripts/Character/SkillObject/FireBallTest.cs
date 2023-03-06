using UnityEngine;

public class FireBallTest : MonoBehaviour
{
    CharacterDataBundle characterData;
    Rigidbody rb;
    Transform target;

    bool moveStart;
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }
    public void Set(Transform t, CharacterDataBundle data)
    {
        characterData = data;
        target = t;
        var rot = target.rotation;
        rot.y = 0;
        //transform.LookAt(target);
    }

    public void MoveStart()
    {
        var particles = GetComponentsInChildren<ParticleSystem>();

        foreach (var item in particles)
        {
            item.Play();
        }
        moveStart = true;
    }

    private void FixedUpdate()
    {
        if (moveStart)
        {
            rb.AddForce(transform.forward * 20);
            TargetVisibleCheck();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            other.transform.GetComponent<AttackableUnit>().OnDamage(characterData.data.baseDamage);
        }
    }

    public void TargetVisibleCheck()
    {
        var cam = Camera.main;
        var planes = GeometryUtility.CalculateFrustumPlanes(cam);
        var point = transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
                Destroy(gameObject);
        }
    }

}
