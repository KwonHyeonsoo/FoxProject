using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMoveEnable : MonoBehaviour
{
    [Range(0.1f, 5f)] public float DetectSize = 1f;
    public LayerMask mask;
    DecalProjector DecalProjector;
    float maxDistance = 1;
    bool isHit;
    RaycastHit hit;
    Vector3 pos;
    private void Start()
    {
        DecalProjector = GetComponent<DecalProjector>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;
        pos.y = transform.parent.position.y;
        //isHit
        isHit = Physics.OverlapSphere(pos, DetectSize * 1.5f * transform.lossyScale.x / 2,
                                                       //2f,
                                                       mask).Count() > 0;
        if (isHit)
        {
            DecalProjector.material.SetColor("_BaseColor", Color.red);
            //Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
        }
        else
        {
            DecalProjector.material.SetColor("_BaseColor", Color.white);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        {
            Gizmos.DrawRay(pos, transform.forward * hit.distance);
            Gizmos.DrawWireSphere(pos + transform.forward * hit.distance, DetectSize*  1.5f * transform.lossyScale.x / 2);
            //2f);
        }

    }

    public bool getUnholdEnable()
    {
        return !isHit;
    }
}
