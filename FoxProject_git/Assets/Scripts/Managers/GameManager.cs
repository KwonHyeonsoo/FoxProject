using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {

    }

    
    void Update()
    {
        
    }
}
