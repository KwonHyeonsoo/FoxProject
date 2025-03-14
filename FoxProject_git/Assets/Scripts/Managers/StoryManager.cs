using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
public class StoryManager
{
    GameObject _Boss1, _Boss2;  //addressable로 불러오기 or 리소스 매니저 처리
    public StoryData story1; // 스토리 데이터 (에디터에서 할당하거나 리소스 매니저 처리)
    int currentElementID;
    int currentStoryID;
    bool isEnd = false;
    // Start is called before the first frame update
    public void Start()
    {
        story1 = Managers.Instance.story1;
        currentElementID = currentStoryID = story1.events[0].eventId;
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
    public void InvokeWASD(InputAction.CallbackContext context)
    {
        if (isEnd) return;

        StoryEventCondition eventCondition = StoryEventCondition.WASDdown;
        if (eventCondition == story1.events[currentElementID].evectCondition)
        {

            int id = story1.events[currentElementID].eventId;
            while (id == story1.events[currentElementID].eventId)
            {
                story1.events[currentElementID].ExecuteEvent();
                currentElementID++;
            }
            currentStoryID++;
            if (currentStoryID >= story1.events.Length) isEnd = true;
        }

        
    }

    public void InvokeRIDE()
    {
        if (isEnd) return;

        StoryEventCondition eventCondition = StoryEventCondition.OnRide;
        if (eventCondition == story1.events[currentElementID].evectCondition)
        {

            int id = story1.events[currentElementID].eventId;
            while (currentElementID <= story1.events.Length-1 && id == story1.events[currentElementID].eventId)
            {
                story1.events[currentElementID].ExecuteEvent();
                currentElementID++;
            }
            currentStoryID++;
        }

        if (currentStoryID >= story1.events.Length) isEnd = true;

    }
    public void InvokeINTERACTION()
    {
        if (isEnd) return;

        StoryEventCondition eventCondition = StoryEventCondition.Interation;
        if (eventCondition == story1.events[currentElementID].evectCondition)
        {

            int id = story1.events[currentElementID].eventId;
            while (currentElementID <= story1.events.Length - 1 && id == story1.events[currentElementID].eventId)
            {
                story1.events[currentElementID].ExecuteEvent();
                currentElementID++;
            }
            currentStoryID++;
        }

        if (currentStoryID >= story1.events.Length) isEnd = true;

    }

    public void InvokeEvent(int storyId, StoryEventCondition eventCondition)
    {
        if (isEnd) return;

        while (currentElementID <= story1.events.Length - 1 && storyId == story1.events[currentElementID].eventId)
        {
            story1.events[currentElementID].ExecuteEvent();
            currentElementID++;
        }
        currentStoryID++;
        if (currentStoryID >= story1.events.Length) isEnd = true;

    }

    public void InvokeEvent(StoryEventCondition eventCondition)
    {
        if (isEnd) return;

        if (eventCondition == story1.events[currentElementID].evectCondition)
        {
            int id = story1.events[currentElementID].eventId;
            while (id == story1.events[currentElementID].eventId)
            {
                story1.events[currentElementID].ExecuteEvent();
                currentElementID++;
            }
        }
        currentStoryID++;
        if (currentStoryID >= story1.events.Length) isEnd = true;

    }

}
