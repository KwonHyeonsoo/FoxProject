using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UI_VideoPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer vid;

    private void OnEnable()
    {
        Time.timeScale = 0f;
        Managers.gameManager.IsInputLock = true;
        vid.loopPointReached += CheckOver;
    }
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
        if(Managers.gameManager!=null) Managers.gameManager.IsInputLock = false;

        vid.loopPointReached -= CheckOver;
    }
}
