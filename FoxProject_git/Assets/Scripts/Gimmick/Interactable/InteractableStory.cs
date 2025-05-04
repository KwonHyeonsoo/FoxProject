using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStory : InteractableObject
{
    public string text1;
    public string text2;

    public override void Invoke(GameObject playerObject)
    {
        Managers.UI_manager.PrintObjectText(text1, text2);
    }
}
