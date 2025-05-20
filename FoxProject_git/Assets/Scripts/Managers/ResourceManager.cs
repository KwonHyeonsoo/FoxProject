using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public List<Dictionary<string, object>> data_guidelog1;
    public List<Dictionary<string, object>> data_init_paths;    //초기화시 필요한
    public List<Dictionary<string, object>> data_immediate_paths;

    public Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> _immediates = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> _UI = new Dictionary<string, GameObject>();
    public Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    public StoryData currentStory;
    private string _currentStoryName;
    private int maxStoryCount = 5;
    private int currentStoryCount = 1;
    private int story_cursor = 0;
    private int guide_cursor = 0;
    private int immediate_cursor = 0;   // 실시간으로 불러올 리소스 가리키는 커서
    //Addressable
    // Start is called before the first frame update
    public void Start()
    {
        //json 파일 읽어오기(데이터 세이브용)
        Debug.Log("resource awake");

    }

    private void ReadResourceALL()
    {
        currentStoryCount = 1;
        //CSV 읽기
        data_dialogue1 = CSVReader.Read("Dialogue/dialogue1");
        data_guidelog1 = CSVReader.Read("Dialogue/guidelog1");

        data_init_paths = CSVReader.Read("Lists/Init_Resource");
        data_immediate_paths = CSVReader.Read("Lists/Immediate_Resource "+currentStoryCount);
        //data_paths = CSVReader.Read("");

        //Resource 폴더 읽기(preLoad)
        //Init Resource 폴더 읽기
        for (int i = 0; i < data_init_paths.Count; i++)
        {
            string datatype = data_init_paths[i]["TYPE"].ToString();
            switch (datatype)
            {
                case "UI":
                    _UI.Add(data_init_paths[i]["NAME"].ToString(), Resources.Load<GameObject>("Prefabs/UI/" + data_init_paths[i]["NAME"]));
                    break;
                case "GameObject":  //오디오믹서
                    break;
                case "StoryData":
                    _currentStoryName = (string)data_init_paths[i]["NAME"];
                    currentStory = Resources.Load<StoryData>("ScriptableObjects/StoryData/" + data_init_paths[i]["NAME"] +" "+ currentStoryCount);
                    break;
                case "Sound":
                    _audioClips.Add(data_init_paths[i]["NAME"].ToString(), Resources.Load<AudioClip>("TestSound/" + data_init_paths[i]["NAME"]));
                    break;
                case "Dialogue":
                    break;
                case "SaveData":
                    break;
                default:
                    Debug.LogWarning("Wrong Type String Init Resource ID : " + i);
                    break;
            }

        }
        Debug.Log(_UI.Count);
        _immediates.Add("test", Resources.Load<GameObject>("Prefabs/test"));

        //ScriptableObject
        //ScriptsText

        isLoaded = true;
        Managers.eventManager.PostNotification(EVENT_TYPE.InitResourceLoaded, null, null);

    }
    public StoryData GetNewStory()
    {
        if (currentStoryCount > 5) return null;
        Debug.Log(currentStoryCount + " is Loaded");
        data_immediate_paths = CSVReader.Read("Lists/Immediate_Resource " + currentStoryCount);
        immediate_cursor = 0;
        currentStory = Resources.Load<StoryData>("ScriptableObjects/StoryData/" + _currentStoryName + " "+(currentStoryCount++));


        return currentStory;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("ResourceManager OnSceneLoaded");
        //if(isLoaded == false)
        {
            ReadResourceALL();
        }

        Debug.Log("Reset cursor");
        story_cursor = 0;
        guide_cursor = 0;
        immediate_cursor = 0;   // 실시간으로 불러올 리소스 가리키는 커서

    }
public int CheckCurrentStoryID()
    {
        if (data_dialogue1.Count <= story_cursor) return -1;
        return int.Parse(data_dialogue1[story_cursor]["ID"].ToString());
    }

    public int CheckCurrentGuideID()
    {
        if (data_guidelog1.Count <= guide_cursor) return -1;
        return int.Parse(data_guidelog1[guide_cursor]["ID"].ToString());
    }

    public string PassStoryText()
    {
        return data_dialogue1[story_cursor++]["TEXT"].ToString();
    }

    public string PassGuideText()
    {
        return data_guidelog1[guide_cursor++]["TEXT"].ToString();
    }

    //실시간으로 오브젝트 생성할 때
    public GameObject GetGameObject(int ID)
    {
        //while(ID > immediate_cursor)
        //{
        //    immediate_cursor++;
        //}
        if (ID.ToString() == data_immediate_paths[immediate_cursor]["ID"].ToString()) {
            //story id에 해당하는 게임오브젝트의 id를 계산
            if (data_immediate_paths[immediate_cursor]["TYPE"].ToString() == "GameObject")
                //해당 게임 오브젝트 id의 게임오브젝트 반환
                return Resources.Load<GameObject>("Prefabs/" + data_immediate_paths[immediate_cursor++]["NAME"]);//_immediates["test"];

        }
        Debug.LogWarning($"Mismatch ID and Gameobject Type \nCheck up Immediate path file  ID : {ID} immediate_cursor{immediate_cursor}");

        return null;
    }

    public Material getDaySkybox()
    {
        return Resources.Load<Material>("Skybox/DaySky");
    }
    public AudioClip GetGameAudio(int ID)
    {
        if (ID.ToString() == data_immediate_paths[immediate_cursor]["ID"].ToString())
        {
            //story id에 해당하는 게임오브젝트의 id를 계산
            if (data_immediate_paths[immediate_cursor]["TYPE"].ToString() == "Sound")
                //해당 게임 오브젝트 id의 게임오브젝트 반환
                return Resources.Load<AudioClip>("TestSound/" + data_immediate_paths[immediate_cursor++]["NAME"]);//_immediates["test"];

        }
        Debug.LogWarning("Mismatch ID and Gameobject Type \nCheck up Immediate path file");

        return null;
    }
}
