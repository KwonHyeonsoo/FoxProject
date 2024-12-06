using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(Boss_FieldofView))]
public class PlayerView : MonoBehaviour
{
    private Boss_FieldofView view;

    public float StayCount;

    private IEnumerator enumerator;

    [SerializeField]private bool isBossHere;

    private GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<Boss_FieldofView>();
        //enumerator = isStayForSecond(StayCount);
        isBossHere = false;
        boss = GameObject.FindGameObjectWithTag("Boss");
        Debug.Log("boss is");
    }

    // Update is called once per frame
    void Update()
    {
        if (view.getVisibleTarget().Count > 0 && !isBossHere)
        {
            enumerator = isStayForSecond(StayCount);
            StartCoroutine(enumerator);
            isBossHere = true;
            Debug.Log("StartCoroutine");
        }
        else if (view.getVisibleTarget().Count == 0 && isBossHere)
        {
            StopCoroutine(enumerator);
            isBossHere = false;
            Debug.Log("StopCoroutine");
        }

    }

    IEnumerator isStayForSecond(float time)
    {
        yield return new WaitForSeconds(time);
        //Boss look around »£√‚
        isBossHere = false;
        if(boss != null)
            boss.GetComponent<Boss_original>().StartLookaround();
        Debug.Log("StartLookaround");
    }
}
