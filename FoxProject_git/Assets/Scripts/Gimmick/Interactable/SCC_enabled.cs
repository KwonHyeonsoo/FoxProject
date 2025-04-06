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
        if (InputManager.Instance.cooltime > 0) return;
        Debug.Log("Invoke SCCenabled");
        //���� 
        InputManager.Instance.SwitchInput(); //input ��ü
        //
        controller.SwitchPlayer();  //���� scc ������Ʈ ��Ȱ��ȭ/Ȱ��ȭ

        
    }


    public void Invoke()
    {
        controller.SwitchPlayer();

    }

}
