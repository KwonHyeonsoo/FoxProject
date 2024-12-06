using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(LightController))]
public class LightReceiver : MonoBehaviour
{
    LightController controller;
    light_type type;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
        controller = GetComponent<LightController>();
        type = controller.type = light_type.receiver;

    }

    private void LateUpdate()
    {
        hit = controller.getHit();
    }


}
