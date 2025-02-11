using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCC_enabled : InteractableObject
{
    public List<MonoBehaviour> components = new List<MonoBehaviour>();
    public List<GameObject> objs = new List<GameObject>();

    GameObject player;
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Invoke(GameObject playerObject)
    {
        foreach (var com in components)
        {
            com.enabled = !com.enabled;
        }
        foreach (var o in objs)
        {
            o.SetActive(!o.activeSelf);
        }
        player = playerObject;
        player.SetActive(!player.activeSelf);
        if (player.activeSelf) { player.transform.position = transform.position; }
    }
}
