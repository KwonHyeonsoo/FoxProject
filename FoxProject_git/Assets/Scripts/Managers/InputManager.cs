using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonBehaviour<InputManager>
{
    //public static InputManager _instance;
    public static SCC_InputActions carInputActions { get; set; }
    private static PlayerInput playerinput;

    string str = "PlayerActions";
    static bool isPlayer = true;

    static GameObject player;
    static GameObject carPlayer;

    public delegate void delegateSwitch();
    public static delegateSwitch switchPlayer;  //���� ���� ��� Ȱ��/��Ȱ��ȭ

    void Awake()
    {
        if (carInputActions == null)
        {
            carInputActions = new SCC_InputActions();
        }
        if (playerinput == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerinput = player.GetComponent<PlayerInput>();
        }
        //playerinput.actions.FindActionMap("PlayerActions").FindAction("Hold").performed += SwitchInput;
        carInputActions.FindAction("Unride").performed += SwitchInput;
    }
    //���� ����
    public static void SwitchInput(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            Debug.Log("1switchinput" + " isPlayer " + isPlayer);
            

            SwitchInput();
            /*
            //isPlayer = !isPlayer;
            //if (isPlayer)
            //{
            //    carInputActions.Disable();
            //    playerinput.enabled = true;
            //    switchPlayer();//scc
            //    player.SetActive(true);
            //}
            //else
            //{
            //    carInputActions.Enable();
            //    playerinput.enabled = false;
            //    //switchPlayer();//scc
            //    player.SetActive(false);
            //}
            */
        }
        Debug.Log("2switchinput" + " isPlayer " + isPlayer);
    }
    //���� ž��
    public static void SwitchInput()
    {

        Debug.Log("3switchinput" + " isPlayer " + isPlayer);
        isPlayer = !isPlayer;

        if (isPlayer)
        {
            carInputActions.Disable();
            playerinput.enabled = true;
            switchPlayer();//scc
            player.SetActive(true);
        }
        else//ž��
        {
            carInputActions.Enable();
            playerinput.enabled = false;
            //switchPlayer();//scc
            player.SetActive(false);
        }
    }

}
