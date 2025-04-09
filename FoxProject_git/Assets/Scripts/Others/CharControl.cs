using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class CharControl : MonoBehaviour
{

    PlayableDirector pd;
    public TimelineAsset[] ta;

    public static CharControl Instance;


    private void Start()
    {
        
        Instance = this;
        pd = GetComponent<PlayableDirector>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CutScene")
        {
            other.gameObject.SetActive(false);
            pd.Play(ta[0]);
        }
        //사운드 호출

    }

}

