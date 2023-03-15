using UnityEngine;
using System.Collections;


public class csLaser : MonoBehaviour
{

    public float Width = 1.5f; //LineRenderer Width Value
    float CurrentWidth = 0.0f;
    public float Offset = 1.0f; //LineRenderer MainTexture Offset Value 
    public float MaxLength = Mathf.Infinity; 
    public Transform LaserHitEffect; //For Laser Hit Effect.
    public Material _Material; //For LineRenderer Material

    private LineRenderer _LineRenderer; //LineRenderer Value
    private float NowLength; // if Raycast Hit Something, Save Length Information Between this transform , RacastHit's hit point.
    private GameObject _Effect;
    public float MaxTime;
    public float shrinkValue;
    float time;
    
    void Awake()
    {
        if (!(LineRenderer)transform.gameObject.GetComponent("LineRenderer")) 
            this.gameObject.AddComponent<LineRenderer>();

        Transform MakedEffect = Instantiate(LaserHitEffect, transform.position, transform.rotation) as Transform; 
        _Effect = MakedEffect.gameObject; // Set MakeEffect Information To _Effect

        csEffectScene.m_destroyObjects[csEffectScene.inputLocation] = _Effect;
        csEffectScene.inputLocation++;

        _LineRenderer = GetComponent<LineRenderer>(); 
        _LineRenderer.material = _Material;
        CurrentWidth = Width;
        _LineRenderer.startWidth = CurrentWidth;
        _LineRenderer.endWidth = CurrentWidth;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    void Update()
    {
        RaycastHit hit;
        time += Time.deltaTime;

        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxLength)) 
        {
            NowLength = hit.distance+1; 

            if(_Effect)
            { 
                _Effect.transform.position = hit.point; 
                _Effect.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
        }
        else
            NowLength = MaxLength;

        if (time > MaxTime)
        {
            CurrentWidth -= Time.deltaTime * shrinkValue;
            CurrentWidth = Mathf.Clamp01(CurrentWidth);
            _LineRenderer.startWidth = CurrentWidth;
            _LineRenderer.endWidth = CurrentWidth;
            if (CurrentWidth <= 0)
                Destroy(gameObject);
        }

        Vector3 NewPos = this.transform.position + new Vector3(transform.forward.x * (NowLength - 1)
            , transform.forward.y * (NowLength - 1), transform.forward.z * (NowLength - 1));                                                                     
        _LineRenderer.SetPosition(0, transform.position); 
        _LineRenderer.SetPosition(1, NewPos); 
        _LineRenderer.material.SetTextureOffset("_MainTex",
            new Vector2(-Time.time * 10f * Offset, 0.0f));
        _LineRenderer.GetComponent<Renderer>().materials[0].mainTextureScale = new Vector2(NowLength/10, _LineRenderer.GetComponent<Renderer>().materials[0].mainTextureScale.y);
    }
}
