using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    private GameObject hitInstaiated;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    private string activeOffFuncName;

    //private ParticleSystem ps;

    private void Awake()
    {
        //ps = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        activeOffFuncName = nameof(SetActiveOffGameObject);
    }

    void Start()
    {
        hitInstaiated = Instantiate(hit);
        hitInstaiated.SetActive(false);
        //if (flash != null)
        //{
        //    var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
        //    flashInstance.transform.forward = gameObject.transform.forward;
        //    var flashPs = flashInstance.GetComponent<ParticleSystem>();
        //    if (flashPs != null)
        //    {
        //        Destroy(flashInstance, flashPs.main.duration);
        //    }
        //    else
        //    {
        //        var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
        //        Destroy(flashInstance, flashPsParts.main.duration);
        //    }
        //}
        //Destroy(gameObject,5);
    }
    private void OnEnable()
    {
        rb.velocity = transform.parent.forward * speed;
        Invoke(activeOffFuncName, 0.5f);
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        //hitInstaiated.transform.position = other.ClosestPoint();
        //hitInstaiated.SetActive(true);
        //hitInstaiated.GetComponent<ParticleSystem>().Play();

        gameObject.SetActive(false);
    }

    private void SetActiveOffGameObject()
    {
        gameObject.SetActive(false);
    }

    //https ://docs.unity3d.com/ScriptReference/Rigidbody.OnCollisionEnter.html

    //void OnCollisionEnter(Collision collision)
    //{
    //    Lock all axes movement and rotation
    //    rb.constraints = RigidbodyConstraints.FreezeAll;
    //    speed = 0;

    //    ContactPoint contact = collision.contacts[0];
    //    Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
    //    Vector3 pos = contact.point + contact.normal * hitOffset;

    //    if (hit != null)
    //    {
    //        var hitInstance = Instantiate(hit, pos, rot);
    //        if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
    //        else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
    //        else { hitInstance.transform.LookAt(contact.point + contact.normal); }

    //        var hitPs = hitInstance.GetComponent<ParticleSystem>();
    //        if (hitPs != null)
    //        {
    //            //Destroy(hitInstance, hitPs.main.duration);
    //        }
    //        else
    //        {
    //            //var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
    //            //Destroy(hitInstance, hitPsParts.main.duration);
    //        }
    //    }
    //    hitInstaiated.transform.position = pos;
    //    hitInstaiated.SetActive(true);
    //    hitInstaiated.GetComponent<ParticleSystem>().Play();
    //    foreach (var detachedPrefab in Detached)
    //    {
    //        if (detachedPrefab != null)
    //        {
    //            detachedPrefab.transform.parent = null;
    //        }
    //    }
    //    Destroy(gameObject);
    //    gameObject.SetActive(false);
    //    ps.Stop();
    //}
}
