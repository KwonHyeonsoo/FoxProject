using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{

    GameObject _defaultPlayer;
    public GameObject DefaultPlayer 
    { 
        get 
        {
            if (_defaultPlayer == null)
            {
                _defaultPlayer = GameObject.FindGameObjectWithTag("Player");
            }

                return _defaultPlayer; 
        } 
    }
    
    GameObject _carPlayer;
    public GameObject CarPlayer
    {
        get
        {
            if (_carPlayer == null)
                _carPlayer = GameObject.FindGameObjectWithTag("Car");
            return _carPlayer;
        }

    }

    GameObject _boss;

    bool isGameOver;
    public bool IsGameOver
    {
        get { return isGameOver; }
        set { }
    }
    #region General GameManager
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Managers.UI_manager.PopUP_GameOver();
        Managers.soundManager.Stop();
        foreach (AudioSource e  in CarPlayer.GetComponentsInChildren<AudioSource>())
        {
            e.enabled = false;
        }

        foreach ( GameObject e in GameObject.FindGameObjectsWithTag("Boss"))
        {
            e.GetComponent<AudioSource>().Stop();
        }
        //사운드 일지 정지
        //시간 일시 정지
        Time.timeScale = 0f;
        Debug.Log("GameOver!");

    }

    #endregion
}
