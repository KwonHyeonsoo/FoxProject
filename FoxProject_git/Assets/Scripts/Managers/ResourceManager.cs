using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

enum ResourceType
{
    Scripts,
    Prefabs,
    UI,
    Sounds
}
public class ResourceManager
{
    public bool isLoaded = false;

    string _UI_path = "Prefabs/UI/";
    string _gameobject_path = "Prefabs/";
    string _scriptable_path = "ScriptableObjects/StoryData";

    public List<Dictionary<string, object>> data_dialogue1;
    public List<Dictionary<string, object>> data_init_paths;    //초기화시 필요한
    public List<Dictionary<string, object>> data_immediate_paths;

    public Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> _immediates = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> _UI = new Dictionary<string, GameObject>();
    public Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private int story_cursor = 0;

    //Addressable
    // Start is called before the first frame update
    public void Start()
    {
        //json 파일 읽어오기
        Debug.Log("resource awake");
        //CSV 읽기
        data_dialogue1 = CSVReader.Read("Dialogue/dialogue1");
        //data_paths = CSVReader.Read("");

        //Resource 폴더 읽기(preLoad)
        //UI
        _UI.Add ("Canvas",  Resources.Load<GameObject>("Prefabs/UI/Canvas"));
        _UI.Add("GuideText", Resources.Load<GameObject>("Prefabs/UI/_GuideText"));
        _UI.Add("StoryText", Resources.Load<GameObject>("Prefabs/UI/_StoryText"));
        _UI.Add("GameOverPanel", Resources.Load<GameObject>("Prefabs/UI/GameOverPanel"));
        _UI.Add("HoldText", Resources.Load<GameObject>("Prefabs/UI/HoldText"));

        Debug.Log(_UI.Count);
        _immediates.Add("test", Resources.Load<GameObject>("Prefabs/test"));

        //ScriptableObject
        //ScriptsText
        //Sound
        _audioClips.Add("BGM01", Resources.Load<AudioClip>("TestSound/BGM_01"));
        _audioClips.Add("SFX01", Resources.Load<AudioClip>("TestSound/SFX01"));
        _audioClips.Add("SFX02", Resources.Load<AudioClip>("TestSound/SFX02"));

        isLoaded = true;
        Managers.eventManager.PostNotification(EVENT_TYPE.InitResourceLoaded, null , null);

    }
    public int CheckCurrentStoryID()
    {
        if (data_dialogue1.Count <= story_cursor) return -1;
        return int.Parse(data_dialogue1[story_cursor]["ID"].ToString());
    }

    public string PassStoryText()
    {
        return data_dialogue1[story_cursor++]["TEXT"].ToString();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ResourcePreLoad()
    {
        //UI
        //ScriptableObject
        //ScriptsText
        //Sound
    }
    
    public GameObject GetGameObject(int ID)
    {
        //story id에 해당하는 게임오브젝트의 id를 계산

        //해당 게임 오브젝트 id의 게임오브젝트 반환
        return _immediates["test"];
    }
}
