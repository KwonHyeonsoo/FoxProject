using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum light_type { emitter, receiver, reflector };

public class LightController : MonoBehaviour
{ 
    [HideInInspector] public light_type type;
    private RaycastHit hit;

    [SerializeField] bool isClear =false;

    public void ReceiveRay(RaycastHit hit)
    {
        hit = this.hit;
    }

    public RaycastHit getHit() { return hit;  }
    public light_type getLightType()
    {
        return type;
    }

    public void LightTrigger(RaycastHit hit)
    {
        hit = this.hit;

        switch (type) 
        {
            case (light_type.receiver):
                isClear = true;
                break;
            default:
                break;

        }

    }
}
