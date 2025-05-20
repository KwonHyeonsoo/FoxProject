using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class ObjectPerform : MonoBehaviour
{
    private Animator animator;
    private Animation _animation;
    private void Start()
    {
        animator = GetComponent<Animator>();
        _animation = GetComponent<Animation>(); 
        if(animator != null)
        {
            animator.enabled = false;
        }
        if (_animation != null) _animation.enabled = false;
    }
    private void MovingPerform( )
    {
        Debug.Log("Perform");
        if (_animation != null)
        {
            _animation.enabled = true;
            _animation.Play();

        }
        GetComponent<AudioSource>()?.Play();

        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            MovingPerform();
        }
    }

    public void MovingPerform(Transform trans)
    {
        Debug.Log("Perform");

        if (animator != null)
        {
            animator.enabled = true;
        }
        else if (_animation != null)
        {
            _animation.enabled = true;
            _animation.Play();

        }
        else
        {
            transform.DOMove(trans.position, 0.1f).SetEase(Ease.InCirc);
            transform.DORotate(trans.rotation.eulerAngles, 0.1f).SetEase(Ease.InCirc);
        }

    }
}
