using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        isEngineStart = isPlayer = true;
        audio = GetComponent<SCC_Audio>();
        defaultMaxVolume = audio.maximumVolume;
    }
    void Start()
    {
        InputManager.Instance.switchPlayer = SwitchPlayer;
        InputManager.Instance.carInputActions.FindAction("Start").performed 
            += EngineStartInputaction;
        
        
        //SwitchPlayer();
        EngineStart();
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
        foreach (var com in playerComponents)
        {
            com.enabled = !com.enabled;
        }
        foreach (var o in playerObjs)
        {
            o.SetActive(!o.activeSelf);
        }

    }
    #endregion

    #region ���˽� �ӵ� ����

    #endregion
}
