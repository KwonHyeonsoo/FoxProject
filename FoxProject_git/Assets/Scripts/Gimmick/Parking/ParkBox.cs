using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    public bool isClear;
    public int gimmickIndex;
    public GimmickChecker checker;

    public bool ReturnTrue()    //부모오브젝트에게 자신의 기믹 상태가 true임을 알려주는 함수

    public bool ReturnFalse()   

*/
public class ParkBox : GimmickAbstract
{
    public float size = 1;
    public LayerMask mask;

    void Start()
    {
        checker = transform.GetComponentInParent<GimmickChecker>();
    }
        // Update is called once per frame
        void FixedUpdate()
    {
        if (Physics.CheckBox(transform.position, transform.lossyScale / 2f * size, Quaternion.identity, mask))
        {
            ReturnTrue();
        }
        else ReturnFalse();
    }

    void OnDrawGizmos()
    {

        bool hit = Physics.CheckBox(transform.position, transform.lossyScale/2f, Quaternion.identity, mask);

        if (hit)
        {
            Gizmos.color = Color.red;
            //Gizmo.DrawRay(origin, direction * hitInfo.distance, color)
            Gizmos.DrawWireCube(transform.position, transform.lossyScale / 2f*size);

        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + Vector3.up * 0, transform.lossyScale / 2f*size);
        }

    }
}
