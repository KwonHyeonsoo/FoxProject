using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : SingletonBehaviour<Managers>
{
    public StoryData story1;
    #region SingletonBehaviour
    /*    

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    private static T _instance = null;
    private static readonly object Lock = new object();
    public static T Instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            lock (Lock)
            {
                if (_instance != null)
                    return _instance;

                // Search for existing instance.
                _instance = (T)FindObjectOfType(typeof(T));
                // Create new instance if one doesn't already exist.
                if (_instance != null) return _instance;


                // Need to create a new GameObject to attach the singleton to.
                var singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString();

                // Make instance persistent.
                DontDestroyOnLoad(singletonObject);

                return _instance;
            }
        }
    }
    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError(string.Format("허용되지 않은 중복 인스턴스 => {0}", typeof(T)));
            Destroy(this);
            return;
        }

        _instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    */
    #endregion

    #region Manager List
    //게임 매니저
    GameManager _gameManager = new GameManager();
    public static GameManager gameManager { get { return Instance._gameManager;  } }
    //스토리 진행 매니저
    StoryManager _storyManager = new StoryManager();
    public static StoryManager storyManager { get {return Instance._storyManager;} }
    //씬 매니저
    //사운드 매니저
    //리소스 매니저
    //세이브 시스템

    //(취소)input Manager
    #endregion

    void Start()
    {
        _name = "Managers";
        _storyManager.Start();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
