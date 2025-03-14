using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryData", menuName = "Story/Story Data")]
public class StoryData : ScriptableObject
{
    public StoryEvent[] events; // 스토리 이벤트 배열
}
