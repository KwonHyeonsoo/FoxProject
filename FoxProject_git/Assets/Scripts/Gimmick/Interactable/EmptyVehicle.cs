using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyVehicle : InteractableObject
{
    public AudioClip clip;
    AudioSource source;
    public override void Invoke(GameObject playerObject)
    {
        if(source == null) source = GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }
}
