using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StoryEventCondition
{
    WASDdown, OnEnterPoint, OnRide, ClearPuzzle, Interation, The_end
}
public enum StoryEventFunction
{
    UI_PlayVideo, UI_StoryText, UI_GuideText,
    Gameobject_Active, Gameobject_Deactive, Gameobject_Perform,
    Save,
    Camera_Moving,
    Sound_PlaySound,
    DeathTimer,
    TurnMorning,
    OutlineShaderSwtich,
    Input_Lock, Input_Unlock
}



[System.Serializable]
public class StoryEvent
{
    public int eventId; // 이벤트 id
    public StoryEventCondition evectCondition;  //이벤트가 실행될 조건
    public StoryEventFunction eventFunction;    //이벤트 실행할 함수
    public int delayTime = 0;
    public Vector3 targetTransform; // 이동할 위치
    public bool isCutscene; // 컷신 여부

    // ⭐ 실행 함수 추가
    public void ExecuteEvent()
    {
        Debug.Log($"스토리 진행: {eventId} - {evectCondition}=>{eventFunction}");

        if (isCutscene)
        {
            Debug.Log("컷신 시작!");
            return;
        }


    }
}
