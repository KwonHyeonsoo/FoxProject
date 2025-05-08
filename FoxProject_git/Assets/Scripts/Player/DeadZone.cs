using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DeadZone : MonoBehaviour
{
    public float size = 1;
    public LayerMask mask;

    void FixedUpdate()
    {
        if (Physics.CheckBox(transform.position, transform.lossyScale / 2f * size, Quaternion.identity, mask))
        {
            Time.timeScale = 0;
            if(this.gameObject.CompareTag("Boss"))
                Managers.gameManager.CarPlayer.GetComponent<PlayableDirector>().Play();
            else
            Managers.gameManager.GameOver();
            gameObject.transform.parent.gameObject?.SetActive(false);
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
            Gizmos.color = UnityEngine.Color.green;
            Gizmos.DrawWireCube(transform.position + Vector3.up * 0, transform.lossyScale * size);
        }

    }
}
