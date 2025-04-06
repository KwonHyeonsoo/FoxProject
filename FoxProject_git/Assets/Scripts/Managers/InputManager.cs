using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public SCC_InputActions carInputActions; //차량 인풋 액션- 새로 생성
    private PlayerInput playerinput;    //인간 인풋 액션 - 플레이어에서 가져옴

    bool isPlayer = true;
    GameObject player;
    //GameObject carPlayer;

    public delegate void delegateSwitch();
    public delegateSwitch switchPlayer;  //차량 기능 활성/비활성화

    public float cooltime { get; set; }

    public static InputManager Instance;
    void Awake()
    {
       // base.Awake();
        //_name = "InputManager";
        Instance = this;

        if (carInputActions == null)
        {
            carInputActions = new SCC_InputActions();   //차량 인풋 액션 생성
        }
        player = Managers.gameManager.DefaultPlayer;    //Player 찾아서 player input 가져오기//나중에 gamemanager에서 가져오느게 나을듯
        playerinput = player.GetComponent<PlayerInput>();

       
    }

    private void Start()
    {
        //playerinput.actions.FindActionMap("PlayerActions").FindAction("Hold").performed += SwitchInput;
        carInputActions.FindAction("Unride").performed += SwitchInput;//unride할 때 인풋액션 교체 함수 추가
        carInputActions.FindAction("Throttle").performed -= Managers.storyManager.InvokeWASD;
        carInputActions.FindAction("Steering").performed -= Managers.storyManager.InvokeWASD;
        carInputActions.FindAction("Throttle").performed += Managers.storyManager.InvokeWASD;
        carInputActions.FindAction("Steering").performed += Managers.storyManager.InvokeWASD;
        playerinput.actions.FindAction("Move").performed += Managers.storyManager.InvokeWASD;
    }
    //차량 하차

    private void Update()
    {
        cooltime -= Time.deltaTime;
    }
    private void OnDestroy()
    {
        carInputActions.FindAction("Unride").performed -= SwitchInput;//unride할 때 인풋액션 교체 함수 추가
        carInputActions.Disable();
        
    }
    public void SwitchInput(InputAction.CallbackContext context)
    {
        if (cooltime > 0) return;
        if (context.performed)
        {
            SwitchInput();//인풋 액션 교체
       
        }
    }
    
    //차량 탑승할 때 인풋액션 교체
    public void SwitchInput()
    {
        if (cooltime > 0) return;

        cooltime = 1;
        Debug.Log("Invoke inputmanager");
        //Debug.Log("3switchinput" + " isPlayer " + isPlayer);
        isPlayer = !isPlayer;

        if (isPlayer)
        {
            carInputActions.Vehicle.Disable();
            playerinput.enabled = true;
            switchPlayer();//scc
            player.transform.position = Managers.gameManager.CarPlayer.transform.position + Managers.gameManager.CarPlayer.transform.right *-2;
            player.SetActive(true);

            Debug.Log(Managers.gameManager.CarPlayer.transform.position);
        }
        else//탑승
        {
            carInputActions.Vehicle.Enable();
            playerinput.enabled = false;
            //switchPlayer();//scc
            player.SetActive(false);
        }
    }

}
