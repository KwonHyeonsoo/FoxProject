using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

//Awake(싱글톤 override) -> Enable -> 씬로드 -> Start -> 씬종료
//  -> (반복) -> Awake -> 씬로드 -> Start
public class Managers : SingletonBehaviour<Managers>
{
    public StoryData story1;
    public GameObject Canvas;
    public GameObject stroy;
    public GameObject hold;
    public GameObject guide;
    public GameObject gameoverPOPUP;
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

    //UI 매니저
    UI_Manager _ui_manager = new UI_Manager();
    public static UI_Manager UI_manager { get { return Instance._ui_manager; } }


    //씬 매니저
    //사운드 매니저
    //리소스 매니저
    ResourceManager _rescourceManager = new ResourceManager();
    public static ResourceManager resourceManager { get { return Instance._rescourceManager; } }
    //세이브 시스템

    //(취소)input Manager
    #endregion

    void Start()
    {
        Debug.Log("Manager.start");
        _name = "Managers";
        _storyManager.Start();
        _ui_manager.Start();
        _rescourceManager.Start();
    }

    void Update()
    {
        storyManager.Update();
    }

    void OnEnable()
    {
        Debug.Log("Manager.Enable");
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneLoaded += _ui_manager.OnSceneLoaded;
        SceneManager.sceneLoaded += _storyManager.OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        Debug.Log("OnSceneLoaded: " + scene.name + "|| LoadSceneMode: "+ mode);
    }
    void OnSceneUnLoaded(Scene scene)
    {
        Time.timeScale = 1f;
        Debug.Log("OnSceneUnLoad" );
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded -= _ui_manager.OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
    }


    #region General GameManager
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        _ui_manager.PopUP_GameOver();
        //사운드 일지 정지
        //시간 일시 정지
        Time.timeScale = 0f;
        Debug.Log("GameOver!");

    }

    #endregion
}
