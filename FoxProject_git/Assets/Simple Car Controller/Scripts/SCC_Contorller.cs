using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
/*
 * 
 * <<���>>
 * 1. Ư�� ��ü ���˽� �ӵ� ������
 * 2. �÷��̾� ��ü ���
 * 3. �õ�&�Ҹ� on off
 * 
 */

public class SCC_Contorller : MonoBehaviour
{
    private bool isStartWithCar = false;
    [HideInInspector]public bool isEngineStart;
    [HideInInspector]public bool isPlayer;

    public List<MonoBehaviour> engineComponents = new List<MonoBehaviour>();
    public List<MonoBehaviour> playerComponents = new List<MonoBehaviour>();
    public List<GameObject> playerObjs = new List<GameObject>();
    public List<GameObject> engineObjs = new List<GameObject>();
    private SCC_Audio audio;
    private float defaultMaxVolume;

    private void Awake()
    {
        isEngineStart = isPlayer = !isStartWithCar;
        audio = GetComponent<SCC_Audio>();
        defaultMaxVolume = audio.maximumVolume;
    }
    void Start()
    {

        InputManager.Instance.switchPlayer += SwitchPlayer;

        InputManager.Instance.carInputActions.FindAction("Start").performed += EngineStartInputaction;

        //���� �Ŵ������� ���� �ڵ��� �Ҹ��� �ñ��

        //SwitchPlayer();

        Vector3[] t = Managers.resourceManager.getInitalTransform();

        if (transform.position.x < -1000)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.MovePosition(t[0] + Vector3.right * 5f);
            if (t[1].x == 1)
            {
                rb.MovePosition(new Vector3(-1459f, 37.6f, -48.39999f));

            }
            rb.MoveRotation(Quaternion.Euler(t[1]));

        }
        if (!isStartWithCar) EngineStart();
    }
    private void OnEnable()
    {
        GetComponent<PlayableDirector>().stopped += Managers.gameManager.OnGameOver;
    }
    private void OnDisable()
    {
        InputManager.Instance.carInputActions.FindAction("Start").performed -= EngineStartInputaction;
        InputManager.Instance.switchPlayer -= SwitchPlayer;
    }

    #region �õ�
    public void EngineStartInputaction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EngineStart();

        }
    }

    private void EngineStart()
    {
        isEngineStart = !isEngineStart;

        SwitchEnabling();

        if (isEngineStart)
        {
            //�Ҹ�
            audio.maximumVolume = defaultMaxVolume;
        }
        else
        {
            //�Ҹ�
            audio.maximumVolume = 0;
            //�õ��� ���� ������ ���ߴ�
        }
    }

    public void SwitchEnabling()
    {
        foreach (var com in engineComponents)
        {
            com.enabled = isEngineStart;
        }
        foreach (var o in engineObjs)
        {
            o.SetActive(isEngineStart);
        }

    }
    #endregion

    #region player ��ü
    public void SwitchPlayer()
    {
        if (Managers.gameManager.IsInputLock) return;
        Debug.Log("Controller swtihPalyer");
        foreach (var com in playerComponents)
        {
            com.enabled = !com.enabled;
        }
        foreach (var o in playerObjs)
        {
            o.SetActive(!o.activeSelf);
        }
        //Managers.storyManager.InvokeRIDE();
    }
    #endregion

    #region ���˽� �ӵ� ����

    #endregion
}
