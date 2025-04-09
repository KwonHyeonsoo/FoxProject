using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StoryEventCondition
{
    WASDdown, OnRide, ClearPuzzle, Interation, The_end, EnterZone, Blank
}
public enum StoryEventFunction
{
    UI_PlayVideo, UI_StoryText, UI_GuideText,
    Gameobject_Active, Gameobject_Deactive, Gameobject_Perform,
    Save,
    Camera_Moving,
    Play_cinemachine,
    Sound_PlaySound,
    DeathTimer, CancelDeathTimer,
    TurnMorning,
    OutlineShaderSwtich,
    Input_Lock, Input_Unlock, 
    Blank

}



[System.Serializable]
public class StoryEvent
{
    public int eventId; // 이벤트 id
    public StoryEventCondition evectCondition;  //이벤트가 실행될 조건
    public StoryEventFunction eventFunction;    //이벤트 실행할 함수
    public int delayTime = 0;
    public Transform targetTransform; // 이동할 위치
    public string desc;

    // ⭐ 실행 함수 추가
    public void ExecuteEvent()
    {
        Debug.Log($"스토리 진행: {eventId} - {evectCondition}=>{eventFunction}");

        //if (isCutscene)
        //{
        //    Debug.Log("컷신 시작!");
        //    return;
        //}

        switch (eventFunction)
        {
            case StoryEventFunction.UI_StoryText:
                Managers.UI_manager.PrintStoryText(eventId);
                break;
            case StoryEventFunction.UI_GuideText:
                Managers.UI_manager.PrintGuideText(eventId, desc);
                break;
            case StoryEventFunction.Gameobject_Active:
                GameObject.Instantiate<GameObject>(Managers.resourceManager.GetGameObject(eventId), targetTransform.position, targetTransform.rotation);
                break;
            case StoryEventFunction.Gameobject_Deactive:
                Managers.storyManager.GameObjectSetDeActive(eventId);
                break;
            case StoryEventFunction.Gameobject_Perform:
                Managers.storyManager.GameObjectPerform(eventId, targetTransform);
                break;
            case StoryEventFunction.Sound_PlaySound:
                Managers.soundManager.PlayStorySoudnOneShot(eventId);
                break;
            case StoryEventFunction.DeathTimer:
                GameObject go = Managers.Instantiate<GameObject>(Managers.resourceManager.GetGameObject(eventId));
                go.tag = "DeathTimer";
                break;
            case StoryEventFunction.CancelDeathTimer:
                GameObject.FindWithTag("DeathTimer").GetComponent<DeathTimer>().Destroy();
                break;


        }
    }
}
