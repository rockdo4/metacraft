using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SePlayerTrackPosition : SePlayer
{   
    public Transform TargetTransform { get; set; }
    protected override void Update()
    {
        base.Update();
        transform.position = TargetTransform.position;
    }
}
