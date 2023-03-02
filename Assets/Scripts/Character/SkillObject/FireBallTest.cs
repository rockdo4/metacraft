using UnityEngine;

public class FireBallTest : MonoBehaviour
{
    CharacterDataBundle characterData;
    Rigidbody rb;
    AttackableUnit target;

    bool moveStart;
    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }
    public void Set(AttackableUnit t, CharacterDataBundle data)
    {
        characterData = data;
        target = t;
        var rot = target.transform.rotation;
        rot.y = 0;
        transform.LookAt(target.transform);
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
        if(moveStart)
            rb.AddForce(transform.forward * 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            other.transform.GetComponent<AttackableUnit>().OnDamage(characterData.data.baseDamage);
        }
    }
}
