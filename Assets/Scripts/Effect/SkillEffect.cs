using System;
using UnityEngine;

public class SkillEffect : Effect
{
    public Action OnDamage;
    public string targetTag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(targetTag))
        { 
        }
    }

}
