using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_TextObject_Panel : MonoBehaviour
{

    public TextMeshProUGUI title;
    public TextMeshProUGUI content;

    public void PrintText(string t, string c)
    {
        title.text = t;
        content.text = c;
    }
}
