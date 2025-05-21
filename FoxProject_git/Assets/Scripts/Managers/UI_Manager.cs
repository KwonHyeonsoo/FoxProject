using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : IListener
{
    public GameObject UI_Canvas;
    private TextMeshProUGUI holdText;
    private PrintText storyText;
    private PrintText guideText;
    private UI_TextObject_Panel objectText;
    private GameObject videoPanel;
    #region Default Manager Function

    private void MakingUIObject()
    {

        Debug.Log("UI STart");
        UI_Canvas = GameObject.FindObjectOfType<Canvas>().gameObject;//GameObject.Instantiate(Managers.Instance.Canvas);
        UI_Canvas.name = "Canvas_instance";
        GameObject storyobj = GameObject.Instantiate(Managers.resourceManager._UI["_StoryText"], UI_Canvas.transform);
        storyText = storyobj.GetComponent<PrintText>();
        storyobj.SetActive(false);
        GameObject holdobj = GameObject.Instantiate(Managers.resourceManager._UI["HoldText"], UI_Canvas.transform);
        holdText = holdobj.GetComponent<TextMeshProUGUI>();
        holdobj.SetActive(false);
        GameObject guideobj = GameObject.Instantiate(Managers.resourceManager._UI["_GuideText"], UI_Canvas.transform);
        guideText = guideobj.GetComponent<PrintText>();
        guideobj.SetActive(false);
        GameObject textobj = GameObject.Instantiate(Managers.resourceManager._UI["_TextObject_Panel"], UI_Canvas.transform);
        objectText = textobj.GetComponent<UI_TextObject_Panel>();
        textobj.SetActive(false);

        videoPanel = GameObject.Instantiate(Managers.resourceManager._UI["VideoPanel"], UI_Canvas.transform);
        videoPanel.SetActive(false);
    }
    public void Start() {
        //Managers.eventManager.AddListener(EVENT_TYPE.InitResourceLoaded, this);
        MakingUIObject();

    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        Debug.Log("UI_Manager OnSceneLoaded");
        //UI_Canvas = GameObject.Find("Canvas");
        if (Managers.resourceManager.isLoaded)
        {
            MakingUIObject();

        }

    }

    #endregion
    public enum UI_hold_status { DEFAULT, RIDE, INTERACT, HOLD, HOLDING }
    public void UI_holdTextControl(bool activate, UI_hold_status status)
    {

        if (holdText == null) MakingUIObject();
        holdText.gameObject.SetActive(activate);
        switch (status)
        {
            case UI_hold_status.DEFAULT:
                holdText.text = "-";
                break;
            case UI_hold_status.RIDE:
                holdText.text = "[E]탑승";
                break;
            case UI_hold_status.INTERACT:
                holdText.text = "[E]";
                break;
            case UI_hold_status.HOLD:
                holdText.text = "[E]줍기 [좌우클릭]수평회전 [스크롤]수직회전";
                break;
            case UI_hold_status.HOLDING:
                holdText.text = "[E]내려놓기]";
                break;
            default:
                holdText.text = "NONE";
                Debug.LogError("no hold Text");
                break;
        }
    }

    public void PrintStoryText(int eventID)
    {
        if (storyText == null) MakingUIObject();


        storyText.gameObject.SetActive(true);
        storyText.texts.Clear();
        do
        {
            storyText.texts.Add(Managers.resourceManager.PassStoryText());
        }
        while (Managers.resourceManager.CheckCurrentStoryID() == eventID);

        storyText.PrintingText();
    }

    public void PrintGuideText(int eventID,  string text)
    {
        if (guideText == null) MakingUIObject();

        guideText.gameObject.SetActive(true);
        guideText.texts.Clear();
        //do
        //{
            //guideText.texts.Add(Managers.resourceManager.PassGuideText());
            guideText.texts.Add(text);
        //}
        //while (Managers.resourceManager.CheckCurrentGuideID() == eventID);

        guideText.PrintingText();
    }
    public void PrintObjectText(string text1, string text2, Sprite i)
    {
        Managers.gameManager.IsInputLock = true;
        objectText.gameObject.SetActive(true);
        objectText.PrintText(text1, text2,i);
    }
    public void PopUP_GameOver()
    {
        GameObject pop = GameObject.Instantiate(
            Managers.resourceManager._UI["GameOverPanel"], 
            UI_Canvas.transform);
        pop.SetActive(true);

    }

    public void PlayVideo()
    {
        videoPanel.SetActive(true);
    }

    // 이벤트가 발생할 때, 리스너에서 호출할 함수
    public void OnEvent(EVENT_TYPE EventType, Component Sender, object Param = null) 
    {
        Debug.Log("UI REscource done");
    }

}
