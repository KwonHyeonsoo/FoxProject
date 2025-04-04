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
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        Managers.UI_manager.PopUP_GameOver();
        //사운드 일지 정지
        //시간 일시 정지
        Time.timeScale = 0f;
        Debug.Log("GameOver!");

    }

    #endregion
}
