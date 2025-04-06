using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCC_enabled : InteractableObject
{

    private SCC_Contorller controller;
    private void Start()
    {
        controller = GetComponentInParent<SCC_Contorller>();

        Invoke();
    }


    //interactable 상속
    public override void Invoke(GameObject playerObject)
    {
        if (InputManager.Instance.cooltime > 0) return;
        Debug.Log("Invoke SCCenabled");
        //고정 
        InputManager.Instance.SwitchInput(); //input 교체
        //
        controller.SwitchPlayer();  //관련 scc 컴포넌트 비활성화/활성화

        
    }


    public void Invoke()
    {
        controller.SwitchPlayer();

    }

}
