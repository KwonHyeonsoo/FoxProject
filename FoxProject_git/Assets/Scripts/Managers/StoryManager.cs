using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class StoryManager
{
    GameObject _Boss1, _Boss2;  //addressable�� �ҷ����� or ���ҽ� �Ŵ��� ó��
    public StoryData story1; // ���丮 ������ (�����Ϳ��� �Ҵ��ϰų� ���ҽ� �Ŵ��� ó��)
    int currentElementID;   //story �迭 �ε���
    int currentEventExecute;
    int currentStoryID; //���丮 id �ε���
    double delayTime;
    bool isEnd = false;
    bool[] isEventEnd;  //�ش������� �Ϸ�Ǿ�����
    public StoryObjectController controller;
    #region Default Manager Function
    // Start is called before the first frame update
    public void Start()
    {
        //���ҽ� �Ŵ������� _prefabs �ް� id���� ����

        //>>>>>>
        //InitResourceLoad(Managers.resourceManager.GetNewStory());
        //>>>>>>>>

        //storyObjectController �޾ƿ���
        Debug.Log("story1.events[0].eventId;"+story1.events[0].eventId);

        if  (story1.events[0].evectCondition == StoryEventCondition.Blank)
        {
            BlankInvoke();
        }
    }

    void InitResourceLoad(StoryData s)
    {
        if (s == null) return;
        story1 = s;
        isEventEnd = new bool[story1.events.Length];
        currentElementID = currentEventExecute = 0;
        currentStoryID = story1.events[0].eventId;
        for (int i = 0; i < story1.events.Length; i++)
        {
            isEventEnd[i] = false;
        }
        delayTime = 0;
        isEnd = false;

    }
    public void GameObjectSetDeActive(int id) 
    { 
        controller.DeActivateObject(id);
    }

    public void GameObjectPerform(int id, Transform transform)
    {
        controller.PerformObject(id, transform);
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("StoryManager OnSceneLoaded");
        //if (Managers.resourceManager.isLoaded)
        {
            InitResourceLoad(Managers.resourceManager.GetNewStory());

        }

    }


    // Update is called once per frame
    public void Update()
    {
        //Debug.Log(currentEventExecute);
        if (isEnd) { Debug.Log("The Story is over!");
            InitResourceLoad(Managers.resourceManager.GetNewStory());
        }
        //Debug.Log(delayTime);
        if(delayTime > 0)
        {
            delayTime -= Time.deltaTime;
        }
        else
        {
            if (isEventEnd[currentEventExecute])
            {
                story1.events[currentEventExecute].ExecuteEvent();
                delayTime = story1.events[currentEventExecute++].delayTime;
                
                //Debug.Log(delayTime);
            }

            if (currentEventExecute >= story1.events.Length) { isEnd = true; }
        }
    }

    #endregion
    private void StoryExecuteEventsLoop(int StoryID, StoryEventCondition eventCondition)
    {
        if (delayTime > 0) return;
        if (currentElementID >= story1.events.Length) return;

        if (eventCondition == story1.events[currentElementID].evectCondition)
        {
            while (currentElementID < story1.events.Length && StoryID == story1.events[currentElementID].eventId)
            {
                
                isEventEnd[currentElementID] = true;
                currentStoryID = story1.events[currentElementID].eventId;
                currentElementID++;
            }

        }
        BlankInvoke();  //���� �̺�Ʈ�� ������ blank�϶� ����

    }
    public void InvokeWASD(InputAction.CallbackContext context)
    {
        if (Managers.gameManager.IsInputLock) return;
        if (isEnd) return;
        StoryEventCondition eventCondition = StoryEventCondition.WASDdown;
        int id = story1.events[currentElementID].eventId;

        StoryExecuteEventsLoop(id, eventCondition);

    }

    public void InvokeRIDE()
    {
        if (isEnd) return;
        StoryEventCondition eventCondition = StoryEventCondition.OnRide;
        int id = story1.events[currentElementID].eventId;

        StoryExecuteEventsLoop(id, eventCondition);

    }
    public void InvokeINTERACTION()
    {
        if (isEnd) return;
        StoryEventCondition eventCondition = StoryEventCondition.Interation;
        int id = story1.events[currentElementID].eventId;

        StoryExecuteEventsLoop(id, eventCondition);
    }

    public void InvokeEvent(int storyId, StoryEventCondition eventCondition)
    {
        if (isEnd) return;
        StoryExecuteEventsLoop(storyId, eventCondition);

    }

    private void BlankInvoke()
    {
        if (currentElementID >= story1.events.Length) return;

        //Debug.Log("Blank");
        while (currentElementID < story1.events.Length && story1.events[currentElementID].evectCondition == StoryEventCondition.Blank)
        {

            isEventEnd[currentElementID] = true;
            currentStoryID = story1.events[currentElementID].eventId;
            currentElementID++;
        }
    }

}
