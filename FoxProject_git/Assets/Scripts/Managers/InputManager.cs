using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static readonly object m_Lock = new object();
    public SCC_InputActions carInputActions; //���� ��ǲ �׼�- ���� ����
    private PlayerInput playerinput;    //�ΰ� ��ǲ �׼� - �÷��̾�� ������

    bool isPlayer = true;
    GameObject player;
    //GameObject carPlayer;

    public delegate void delegateSwitch();
    public delegateSwitch switchPlayer;  //���� ��� Ȱ��/��Ȱ��ȭ

    public float cooltime { get; set; }

    public static InputManager Instance;
    void Awake()
    {
       // base.Awake();
        //_name = "InputManager";
        Instance = this;
        //Debug.Log("carInputActions+" + carInputActions != null);
        if (carInputActions == null)
        {
            carInputActions = new SCC_InputActions();   //���� ��ǲ �׼� ����
            SCC_InputManager.inputActions = carInputActions;
            //carInputActions.Enable();
        }
        player = Managers.gameManager.DefaultPlayer;    //Player ã�Ƽ� player input ��������//���߿� gamemanager���� ���������� ������
        playerinput = player.GetComponent<PlayerInput>();

       
    }

    private void Start()
    {
        //playerinput.actions.FindActionMap("PlayerActions").FindAction("Hold").performed += SwitchInput;
       // carInputActions.FindAction("Unride").performed += SwitchInput;//unride�� �� ��ǲ�׼� ��ü �Լ� �߰�
        carInputActions.FindAction("Throttle").performed -= Managers.storyManager.InvokeWASD;
        carInputActions.FindAction("Steering").performed -= Managers.storyManager.InvokeWASD;
        carInputActions.FindAction("Throttle").performed += Managers.storyManager.InvokeWASD;
        carInputActions.FindAction("Steering").performed += Managers.storyManager.InvokeWASD;
        playerinput.actions.FindAction("Move").performed += Managers.storyManager.InvokeWASD;
    }
    //���� ����

    private void Update()
    {
        cooltime -= Time.deltaTime;
    }
    private void OnDestroy()
    {
        carInputActions.FindAction("Unride").performed -= SwitchInput;//unride�� �� ��ǲ�׼� ��ü �Լ� �߰�
        carInputActions.Disable();
        
    }
    public void SwitchInput(InputAction.CallbackContext context)
    {
        Debug.Log("unride");
        if (cooltime > 0) return;
        if (context.performed)
        {
            SwitchInput();//��ǲ �׼� ��ü
       
        }
    }
    
    //���� ž���� �� ��ǲ�׼� ��ü
    public void SwitchInput()
    {
        lock (m_Lock)
        {
            if (cooltime > 0) return;
            if (Managers.gameManager.IsInputLock) return;

            cooltime = 1;
            Debug.Log("Invoke inputmanager");
            //Debug.Log("3switchinput" + " isPlayer " + isPlayer);
            isPlayer = !isPlayer;

            if (isPlayer)
            {

                RenderSettings.fogDensity = 0.1f;
                carInputActions.FindAction("Unride").performed -= SwitchInput;//unride�� �� ��ǲ�׼� ��ü �Լ� �߰�

                carInputActions.Vehicle.Disable();

                playerinput.enabled = true;
                switchPlayer();//scc
                player.transform.position = Managers.gameManager.CarPlayer.transform.position + Managers.gameManager.CarPlayer.transform.right * -2;
                player.SetActive(true);

                Debug.Log(Managers.gameManager.CarPlayer.transform.position);
            }
            else//ž��
            {
                RenderSettings.fogDensity = 0.02f;
                carInputActions.Vehicle.Enable();
                carInputActions.FindAction("Unride").performed += SwitchInput;//unride�� �� ��ǲ�׼� ��ü �Լ� �߰�

                playerinput.enabled = false;
                //switchPlayer();//scc
                player.SetActive(false);
            }
        }
    }

}
