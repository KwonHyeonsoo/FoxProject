using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager
{
    public GameObject UI_Canvas;
    private TextMeshProUGUI holdText;
    private PrintText storyText;
    private PrintText guideText;
    public void Start()
    {
        //UI_Canvas = GameObject.Find("Canvas");
        
        UI_Canvas = GameObject.Instantiate(Managers.Instance.Canvas);
        UI_Canvas.name = "Canvas_instance";
        GameObject storyobj = GameObject.Instantiate(Managers.Instance.stroy, UI_Canvas.transform);
        storyText = storyobj.GetComponent<PrintText>();
        storyobj.SetActive(false);
        GameObject holdobj = GameObject.Instantiate(Managers.Instance.hold, UI_Canvas.transform);
        holdText = holdobj.GetComponent<TextMeshProUGUI>();
        holdobj.SetActive(false);
        GameObject guideobj = GameObject.Instantiate(Managers.Instance.guide, UI_Canvas.transform);
        guideText = guideobj.GetComponent<PrintText>();
        guideobj.SetActive(false);


    }
    public enum UI_hold_status { DEFAULT, RIDE, INTERACT, HOLD, HOLDING }
    public void UI_holdTextControl(bool activate, UI_hold_status status)
    {
        holdText.gameObject.SetActive(activate);
        switch (status)
        {
            case UI_hold_status.DEFAULT:
                holdText.text = "-";
                break;
            case UI_hold_status.RIDE:
                holdText.text = "Ride";
                break;
            case UI_hold_status.INTERACT:
                holdText.text = "Act";
                break;
            case UI_hold_status.HOLD:
                holdText.text = "Hold";
                break;
            case UI_hold_status.HOLDING:
                holdText.text = "Holding";
                break;
            default:
                holdText.text = "NONE";
                Debug.LogError("no hold Text");
                break;
        }
    }

    public void PrintStoryText(int eventID)
    {
        storyText.gameObject.SetActive(true);
        storyText.texts.Clear();
        do
        {
            storyText.texts.Add(Managers.resourceManager.PassStoryText());
        }
        while (Managers.resourceManager.CheckCurrentStoryID() == eventID);

        storyText.PrintingText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
