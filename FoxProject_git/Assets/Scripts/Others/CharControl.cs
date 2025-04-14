using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class CharControl : MonoBehaviour
{

    PlayableDirector pd;
    public TimelineAsset[] ta;

    int index = 0;
    public static CharControl Instance;


    private void Start()
    {
        
        Instance = this;
        pd = GetComponent<PlayableDirector>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (ta.Length > index)
        {
            if (other.tag == "CutScene")
            {
                other.gameObject.SetActive(false);
                pd.Play(ta[index++]);
            }
        }
        //사운드 호출

    }

}

