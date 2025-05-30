using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public abstract class GimmickAbstract : MonoBehaviour
{
    [ReadOnly(false)]public bool isClear;
    [ReadOnly(false)] public int gimmickIndex;
    [ReadOnly(false)] public GimmickChecker checker;
    void Start()
    {
        //checker = transform.GetComponentInParent<GimmickChecker>();
    }

    public bool ReturnTrue()    //부모오브젝트에게 자신의 기믹 상태가 true임을 알려주는 함수
    {
        isClear = true;
        checker.SetTrue(gimmickIndex);
        return isClear;
    }
    public bool ReturnFalse()   
    {
        isClear = false;
        checker.SetFalse(gimmickIndex);
        return isClear;
    }
}

public enum light_type { emitter, receiver, reflector };

public class LightController : GimmickAbstract
{ 
    [HideInInspector] public light_type type;
    private RaycastHit hit;
    void Start()
    {
        checker = transform.GetComponentInParent<GimmickChecker>();
        switch (type)
        {
            case light_type.emitter:
                isClear = true;
                break;
            case light_type.receiver:
                isClear = false;
                break;
            case light_type.reflector:
                isClear = true;
                break;
        }
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
                this.ReturnTrue();
                break;
            default:
                break;

        }

    }
    public void LightUntrigger(RaycastHit hit)
    {
        hit = this.hit;

        switch (type)
        {
            case (light_type.receiver):
                isClear = false;
                this.ReturnFalse();
                break;
            default:
                break;

        }

    }
}
