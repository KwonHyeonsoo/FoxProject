using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCredit : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene(3);
    }
}
