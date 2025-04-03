using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class EnterZone : MonoBehaviour
{
    public int eventID;
    public float size = 1;
    public LayerMask mask;

    void FixedUpdate()
    {
        if (Physics.CheckBox(transform.position, transform.lossyScale / 2f * size, Quaternion.identity, mask))
        {
            Managers.storyManager.InvokeEvent(eventID, StoryEventCondition.EnterZone);  

        }

    }
    void OnDrawGizmos()
    {

        bool hit = Physics.CheckBox(transform.position, transform.lossyScale / 2f * size, Quaternion.identity, mask);

        if (hit)
        {
            Gizmos.color = UnityEngine.Color.red;
            //Gizmo.DrawRay(origin, direction * hitInfo.distance, color)
            Gizmos.DrawWireCube(transform.position, transform.lossyScale * size);

        }
        else
        {
            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawWireCube(transform.position + Vector3.up * 0, transform.lossyScale * size);
        }

    }
}
