using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 15f;    
    private Rigidbody rb;

    private string activeOffFuncName;   

    private void Awake()
    {      
        rb = GetComponent<Rigidbody>();
        activeOffFuncName = nameof(SetActiveOffGameObject);
    }

    private void OnEnable()
    {
        transform.forward = transform.parent.forward;
        rb.velocity = transform.forward * speed;
        Invoke(activeOffFuncName, 0.25f);
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {       
        gameObject.SetActive(false);
    }

    private void SetActiveOffGameObject()
    {
        gameObject.SetActive(false);
    }

    //private void OnBecameInvisible()
    //{
    //    gameObject.SetActive(false);
    //}
}
