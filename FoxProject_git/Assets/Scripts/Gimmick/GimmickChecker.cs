using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class GimmickChecker : MonoBehaviour
{
    [ReadOnly] public List<GameObject> gimmicks;
    //List<GimmickAbstract> gimmickAbstracts;
    [ReadOnly] public List<bool> gimmickConditions;
    public int storyID;
    [ReadOnly]public bool allGimmickIsTrue;
    void Start()
    {
        if(gimmicks.Count > 0)
        {
            gimmicks.Clear();
        }
        if (gimmickConditions.Count > 0)
        {
            gimmickConditions.Clear();
        }
        //��� ��ȸ ���鼭 �ε��� �ο�
        for (int i = 0; i< transform.childCount; i++)
        {
            gimmicks.Add(transform.GetChild(i).gameObject);
            var g = gimmicks[i].GetComponent<GimmickAbstract>();
            g.gimmickIndex = i;
            //Debug.Log(g.gimmickIndex + "  " + g.isClear);
            bool b = g.isClear;
            gimmickConditions.Add(b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetTrue(int index)
    {
        gimmickConditions[index] = true;

        bool booleans = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            booleans = booleans && gimmickConditions[i];
        }
        allGimmickIsTrue = booleans;

        if (allGimmickIsTrue) Managers.storyManager.InvokeEvent(storyID, StoryEventCondition.ClearPuzzle);
    }
    public void SetFalse(int index)
    {
        gimmickConditions[index] = false;

        bool booleans = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            booleans = booleans && gimmickConditions[i];
        }
        allGimmickIsTrue = booleans;
    }
}
