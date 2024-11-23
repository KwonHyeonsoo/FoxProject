using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LightController))]
public class LightReflector : MonoBehaviour
{
    LightController controller;
    light_type type;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<LightController>();
        type = controller.type;
    }

    private void LateUpdate()
    {
        hit = controller.getHit();
    }
}
