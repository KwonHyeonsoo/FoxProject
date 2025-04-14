using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class ObjectPerform : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if(animator != null)
        {
            animator.enabled = false;
        }
    }
    public void MovingPerform(Transform trans)
    {
        Debug.Log("Perform");

        if (animator != null)
        {
            animator.enabled = true;
        }
        else
        {
            transform.DOMove(trans.position, 0.1f).SetEase(Ease.InCirc);
            transform.DORotate(trans.rotation.eulerAngles, 0.1f).SetEase(Ease.InCirc);
        }

    }
}
