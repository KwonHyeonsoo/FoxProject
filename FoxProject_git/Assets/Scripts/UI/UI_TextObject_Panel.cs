using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TextObject_Panel : MonoBehaviour
{

    public TextMeshProUGUI title;
    public TextMeshProUGUI content;
    public Image image;
    public void PrintText(string t, string c, Sprite i)
    {
        title.text = t;
        content.text = c;
        image.sprite = i;
    }

    private void OnDisable()
    {
        Managers.gameManager.IsInputLock = false;
    }
}
