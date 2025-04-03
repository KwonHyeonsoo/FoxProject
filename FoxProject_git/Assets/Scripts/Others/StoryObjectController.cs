using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryObjectController : MonoBehaviour
{
    [ReadOnly] public List<GameObject> GameObjects;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObjects.Count > 0)
        {
            GameObjects.Clear();
        }
  
        //��� ��ȸ ���鼭 �ε��� �ο�
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObjects.Add(transform.GetChild(i).gameObject);
        }
        Managers.storyManager.controller = this;
    }

    public void DeActivateObject(int ID)
    {
        foreach (GameObject e in GameObjects)
        {
            if (e.name == ID.ToString())
            {
                e.SetActive(false);
            }
        }
    }
    public void PerformObject(int ID, Transform transform)
    {
        foreach (GameObject e in GameObjects)
        {
            if (e.name == ID.ToString())
            {
                e.GetComponent<ObjectPerform>()?.MovingPerform(transform);
            }
        }

    }
}
