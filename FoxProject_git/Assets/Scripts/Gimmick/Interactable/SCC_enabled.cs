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


    //interactable ���
    public override void Invoke(GameObject playerObject)
    {
        //���� 
        InputManager.Instance.SwitchInput(); //input ��ü
        //
        controller.SwitchPlayer();

        
    }


    public void Invoke()
    {
        controller.SwitchPlayer();

    }

}
