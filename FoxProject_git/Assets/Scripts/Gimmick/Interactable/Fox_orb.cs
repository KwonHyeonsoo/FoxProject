using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Fox_orb : InteractableObject
{
    public GameObject black_clyliner;
    public override void Invoke(GameObject playerObject)
    {
        //검은색 원기둥 활성화 & 원기둥이 플레이어와 접촉히 outline off
        Instantiate(black_clyliner, transform);
        //시네머신, 여우 손 애니메이션, 엔딩크레딧
        GetComponent<PlayableDirector>().Play();
    }
}
