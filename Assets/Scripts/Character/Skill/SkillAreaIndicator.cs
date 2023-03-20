using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class SkillAreaIndicator : MonoBehaviour
{
    public SkillTargetType TargetType { get; set; }
    public MeshRenderer Renderer { get; private set; }

    protected HashSet<Collider> unitsInArea = new();

    public bool isTriggerEnter = false;

    public bool IsTrackTarget { set { isTrackTarget = value; } }
    private bool isTrackTarget = false;
    public Transform TrackTransform { get { return trackTransform; } set { trackTransform = value; } }
    private Transform trackTransform;
    public bool IsTransActor { set { isTransActor = value; } }
    private bool isTransActor = false;

    public Transform ActorTransform
    {
        get { return actorTransform; }
        set
        {
            actorTransform = value;
            agent = actorTransform.GetComponent<NavMeshAgent>();
            Logger.Debug("agent setted");
        }
    }
    private Transform actorTransform;
    private NavMeshAgent agent;

    private bool onTrans = false;
    public float transTime = 0.8f;
    private float divTransTime;
    private float transTimer = 0f;

    private Vector3 startPos;
 
    protected virtual void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
        divTransTime = 1 / transTime;
    }
    private void Update()
    {
        TryTrackTarget();
        TryTransActor();
    }
    private void TryTrackTarget()
    {
        if (!isTrackTarget)
            return;

        if (trackTransform != null)
            transform.position = trackTransform.position + Vector3.up * 0.1f;
    }
    private void TryTransActor()
    {        
        if(!isTransActor) 
            return;        

        if (!onTrans)
            return;        

        TransActor();

    }
    public void StartTransActor()
    {
        onTrans = true;
        startPos = actorTransform.position;
        transTimer = 0f;
        agent.isStopped = true;        
    }
    private void TransActor()
    {        
        transTimer += Time.deltaTime;        
        actorTransform.position = Vector3.Lerp(startPos, transform.position, transTimer * divTransTime);        
        if (transTimer > transTime)
            EndTransActor();
    }
    private void EndTransActor()
    {
        onTrans = false;         
        agent.isStopped = false;        
    }
    //private void SetTransActorValueStartOrEnd(bool trueIsStart)
    //{
    //    onTrans = trueIsStart;
    //    agent.isStopped = trueIsStart;
    //    transTimer = 0f;
    //}
    public virtual void SetScale(float x, float y, float z = 1f)
    {
        transform.localScale = new Vector3(x, y, z);        
    }
    private void OnEnable()
    {
        //if(meshRenderer == null)
        //    meshRenderer = GetComponent<MeshRenderer>();

        Renderer.enabled = true;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        switch (TargetType)
        {
            case SkillTargetType.Enemy:
                if (other.CompareTag("Enemy"))
                {
                    IsColliderIn(true, other);
                }
                break;
            case SkillTargetType.Friendly:
                if (other.CompareTag("Hero"))
                {
                    IsColliderIn(true, other);
                }
                break;
        }
        isTriggerEnter = true;
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        switch (TargetType)
        {
            case SkillTargetType.Enemy:
                if (other.CompareTag("Enemy"))
                {
                    IsColliderIn(false, other);
                }
                break;
            case SkillTargetType.Friendly:
                if (other.CompareTag("Hero"))
                {
                    IsColliderIn(false, other);
                }
                break;
        }
    }
    protected virtual void IsColliderIn(bool isIn, Collider other)
    {
        other.GetComponent<Outline>().enabled = isIn;

        if (isIn)
            unitsInArea.Add(other);
        else
            unitsInArea.Remove(other);
    }
    private void OnDisable()
    {
        foreach (var unit in unitsInArea)
        {       
            if(unit != null)
                unit.GetComponent<Outline>().enabled = false;
        }
        unitsInArea.Clear();        
    } 
    public List<AttackableUnit> GetUnitsInArea()
    {
        List<AttackableUnit> container = new(unitsInArea.Count);

        foreach (Collider collider in unitsInArea)
        {
            AttackableUnit attackableUnit = collider.GetComponent<AttackableUnit>();
            container.Add(attackableUnit);
        }
        return container;
    }
}
