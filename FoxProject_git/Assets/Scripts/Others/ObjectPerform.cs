using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class ObjectPerform : MonoBehaviour
{

    public void MovingPerform(Transform trans)
    {
        Debug.Log("Perform");
        transform.DOMove(trans.position, 0.1f).SetEase(Ease.InCirc);
        transform.DORotate(trans.rotation.eulerAngles, 0.1f).SetEase(Ease.InCirc);

    }
}
