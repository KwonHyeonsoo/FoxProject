using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * 1. delegate 변수를 만든다.
 * 2. 하위 컴포넌트에서 interatableObject 컴포넌트를 불러와서
 * 3. delegate 변수에 자신의 함수를 넣는다.
 */
public abstract class InteractableObject : MonoBehaviour
{
    private int layerAsLayerMask;
    public  LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        tag = "Interactable";
        layerAsLayerMask = layerMask;
    }

    public abstract void Invoke(GameObject playerObject);


}
