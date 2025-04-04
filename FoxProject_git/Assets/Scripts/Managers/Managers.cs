using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Awake(�̱��� override) -> Enable -> ���ε� -> Start -> ������
//  -> (�ݺ�) -> Awake -> ���ε� -> Start
public class Managers : SingletonBehaviour<Managers>
{

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
            Debug.LogError(string.Format("������ ���� �ߺ� �ν��Ͻ� => {0}", typeof(T)));
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
    //���� �Ŵ���
    GameManager _gameManager = new GameManager();
    public static GameManager gameManager { get { return Instance._gameManager;  } }
    //���丮 ���� �Ŵ���
    StoryManager _storyManager = new StoryManager();
    public static StoryManager storyManager { get {return Instance._storyManager;} }

    //UI �Ŵ���
    UI_Manager _ui_manager = new UI_Manager();
    public static UI_Manager UI_manager { get { return Instance._ui_manager; } }


    //�� �Ŵ���
    //���� �Ŵ���
    SoundManager _soundManager = new SoundManager();
    public static SoundManager soundManager { get { return Instance._soundManager; } }
    //���ҽ� �Ŵ���
    ResourceManager _resourceManager = new ResourceManager();
    public static ResourceManager resourceManager { get { return Instance._resourceManager; } }
    //���̺� �ý���

    //�̺�Ʈ �Ŵ���
    EventManager _eventManager = new EventManager();
    public static EventManager eventManager { get { return Instance._eventManager; } }
    //(���)input Manager
    #endregion

    void Start()
    {
        Debug.Log("Manager.start");
        _name = "Managers";

        Managers.eventManager.AddListener(EVENT_TYPE.InitResourceLoaded, _ui_manager);


        _resourceManager.Start();

        _ui_manager.Start();
        _soundManager.Start();
        _storyManager.Start();

    }

    void Update()
    {
        storyManager.Update();
    }

    void OnEnable()
    {
        Debug.Log("Manager.Enable");
        // �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�.
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneLoaded += _resourceManager.OnSceneLoaded;
        SceneManager.sceneLoaded += _ui_manager.OnSceneLoaded;
        SceneManager.sceneLoaded += _storyManager.OnSceneLoaded;
        //SceneManager.sceneLoaded += _eventManager.OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    // ü���� �ɾ �� �Լ��� �� ������ ȣ��ȴ�.
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
        SceneManager.sceneLoaded -= _storyManager.OnSceneLoaded;
        SceneManager.sceneLoaded -= _resourceManager.OnSceneLoaded;
        //SceneManager.sceneLoaded -= _eventManager.OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;


    }



}
