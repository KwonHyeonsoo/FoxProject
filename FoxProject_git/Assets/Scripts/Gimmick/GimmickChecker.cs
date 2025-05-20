using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class GimmickChecker : MonoBehaviour
{

    [ReadOnly] public List<GameObject> gimmicks;
    //List<GimmickAbstract> gimmickAbstracts;
    [ReadOnly] public List<bool> gimmickConditions;
    public int storyID;
    public bool allGimmickIsTrue;
    public bool isCutscene;
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
        //기믹 순회 돌면서 인덱스 부여
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
        if (allGimmickIsTrue) return;

        gimmickConditions[index] = true;

        bool booleans = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            booleans = booleans && gimmickConditions[i];
        }
        allGimmickIsTrue = booleans;

        if (allGimmickIsTrue) {
            Debug.Log("ClearPuzzle");
            Managers.storyManager.InvokeEvent(storyID, StoryEventCondition.ClearPuzzle);
            Managers.soundManager.PlaySoundOneShot(SoundManager.OneShotSound.ClearSound);
            if (isCutscene) 
            { // 컷신 재생
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
    public void SetFalse(int index)
    {
        if (allGimmickIsTrue) return;

        gimmickConditions[index] = false;

        bool booleans = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            booleans = booleans && gimmickConditions[i];
        }
        allGimmickIsTrue = booleans;
    }
}
