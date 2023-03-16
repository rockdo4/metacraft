using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        hit = Instantiate(hit);
        hit.SetActive(false);
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
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

 //   void FixedUpdate ()
 //   {
	//	if (speed != 0)
 //       {
 //           //rb.velocity = transform.forward * speed;
 //           //transform.position += transform.forward * (speed * Time.deltaTime);         
 //       }
	//}

    //https ://docs.unity3d.com/ScriptReference/Rigidbody.OnCollisionEnter.html
 
    void OnCollisionEnter(Collision collision)
    {
        //Lock all axes movement and rotation
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        //speed = 0;

        ContactPoint contact = collision.contacts[0];
        //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

       // if (hit != null)
      //  {
            //var hitInstance = Instantiate(hit, pos, rot);
            //if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            //else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            //else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            //var hitPs = hitInstance.GetComponent<ParticleSystem>();
            //if (hitPs != null)
            //{
            //    //Destroy(hitInstance, hitPs.main.duration);
            //}
            //else
            //{
            //    //var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
            //    //Destroy(hitInstance, hitPsParts.main.duration);
            //}
        //}
        hit.transform.position = pos;
        hit.SetActive(true);
        hit.GetComponent<ParticleSystem>().Play();
        //foreach (var detachedPrefab in Detached)
        //{
        //    if (detachedPrefab != null)
        //    {
        //        detachedPrefab.transform.parent = null;
        //    }
        //}
        //Destroy(gameObject);        
        gameObject.SetActive(false);
        //ps.Stop();
    }
}
