using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCC_enabled : InteractableObject
{
    //public List<MonoBehaviour> components = new List<MonoBehaviour>();
    //public List<GameObject> objs = new List<GameObject>();

    private SCC_Contorller controller;
    private void Start()
    {
        controller = GetComponentInParent<SCC_Contorller>();
        //InputManager.switchPlayer = Invoke;

        Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //interactable ���
    public override void Invoke(GameObject playerObject)
    {
        //���� 
        InputManager.SwitchInput(); //input ��ü
        //
        controller.SwitchPlayer();
        //foreach (var com in components)
        //{
        //    com.enabled = !com.enabled;
        //}
        //foreach (var o in objs)
        //{
        //    o.SetActive(!o.activeSelf);
        //}

        
    }

    //InputManager.switchPlayer
    public void Invoke()
    {
        controller.SwitchPlayer();

    }

}
