using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class CharControl : MonoBehaviour
{

    PlayableDirector pd;
    public TimelineAsset[] ta;

    public string target_tag;

    int index = 0;

    private void Start()
    {
        pd = GetComponent<PlayableDirector>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (ta.Length > index)
        {
            if (other.tag == target_tag)
            {
                pd.Play(ta[index++]);
            }
        }
        //사운드 호출

    }

}

