using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
/*
 * 
 * <<기능>>
 * 1. 특정 물체 접촉시 속도 느려짐
 * 2. 플레이어 교체 기능
 * 3. 시동&소리 on off
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

        //사운드 매니저에게 본인 자동차 소리를 맡긴다

        //SwitchPlayer();
        if(!isStartWithCar) EngineStart();
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

    #region 시동
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
            //소리
            audio.maximumVolume = defaultMaxVolume;
        }
        else
        {
            //소리
            audio.maximumVolume = 0;
            //시동을 끌때 완전히 멈추는
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

    #region player 교체
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

    #region 접촉시 속도 감소

    #endregion
}
