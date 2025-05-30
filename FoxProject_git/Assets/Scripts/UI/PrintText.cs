using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrintText : MonoBehaviour
{

    public List<string> texts;

    TextMeshProUGUI textMesh;

    public void PrintingText()
    {
        if(textMesh == null) { textMesh = GetComponent<TextMeshProUGUI>(); }
        StopCoroutine(PrintingTextLoop());

        StartCoroutine(PrintingTextLoop());
    }

    private IEnumerator PrintingTextLoop()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            textMesh.text = texts[i];
            yield return new WaitForSecondsRealtime(3f + texts[i].Length * 0.2f);
        }
        gameObject.SetActive(false);
        texts.Clear();
    }

    private void OnDestroy()
    {
        //Debug.Log("Destroyt"+gameObject.name);
    }
}
