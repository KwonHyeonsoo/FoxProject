using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryData", menuName = "Story/Story Data")]
public class StoryData : ScriptableObject
{
    public StoryEvent[] events; // ���丮 �̺�Ʈ �迭
}
