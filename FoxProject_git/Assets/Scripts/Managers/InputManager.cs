using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    ////static
    //private static InputManager _instance;
    //public static InputManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            //���� �̹� �����ϴ� InputManager ã��
    //            _instance = FindObjectOfType<InputManager>();

    //            //������ ���� ����
    //            if (_instance == null)
    //            {
    //                GameObject obj = new GameObject("InputManager");
    //                _instance = obj.AddComponent<InputManager>();
    //                DontDestroyOnLoad(obj);
    //            }
    //        }
    //        return _instance;
    //    }
    //}
    //general
    public SCC_InputActions carInputActions; //���� ��ǲ �׼�- ���� ����
    private PlayerInput playerinput;    //�ΰ� ��ǲ �׼� - �÷��̾�� ������

    bool isPlayer = true;
    GameObject player;
    //GameObject carPlayer;

    public delegate void delegateSwitch();
    public delegateSwitch switchPlayer;  //���� ��� Ȱ��/��Ȱ��ȭ

    public static InputManager Instance;
    void Awake()
    {
       // base.Awake();
        //_name = "InputManager";
        Instance = this;

        if (carInputActions == null)
        {
            carInputActions = new SCC_InputActions();   //���� ��ǲ �׼� ����
        }
        player = Managers.gameManager.DefaultPlayer;    //Player ã�Ƽ� player input ��������//���߿� gamemanager���� ���������� ������
        playerinput = player.GetComponent<PlayerInput>();

       
    }

    private void Start()
    {
        //playerinput.actions.FindActionMap("PlayerActions").FindAction("Hold").performed += SwitchInput;
        carInputActions.FindAction("Unride").performed += SwitchInput;//unride�� �� ��ǲ�׼� ��ü �Լ� �߰�
        carInputActions.FindAction("Throttle").performed -= Managers.storyManager.InvokeWASD;
        carInputActions.FindAction("Steering").performed -= Managers.storyManager.InvokeWASD;
        carInputActions.FindAction("Throttle").performed += Managers.storyManager.InvokeWASD;
        carInputActions.FindAction("Steering").performed += Managers.storyManager.InvokeWASD;
        playerinput.actions.FindAction("Move").performed += Managers.storyManager.InvokeWASD;
    }
    //���� ����
    public void SwitchInput(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            //Debug.Log("1switchinput" + " isPlayer " + isPlayer);
            SwitchInput();//��ǲ �׼� ��ü
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
        //Debug.Log("2switchinput" + " isPlayer " + isPlayer);
    }
    
    //���� ž���� �� ��ǲ�׼� ��ü
    public void SwitchInput()
    {

        //Debug.Log("3switchinput" + " isPlayer " + isPlayer);
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
