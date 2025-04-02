using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_gameover : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(OnClickRestart);
    }
    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
    public void OnClickRestart()
    {
        Managers.soundManager.PlayUIEffectOneShot();
        Managers.Instance.Restart();
        
    }
}
