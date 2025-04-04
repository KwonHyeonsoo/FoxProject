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
        //고정 
        InputManager.Instance.SwitchInput(); //input 교체
        //
        controller.SwitchPlayer();

        
    }


    public void Invoke()
    {
        controller.SwitchPlayer();

    }

}
