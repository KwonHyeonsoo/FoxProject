using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * 1. delegate ������ �����.
 * 2. ���� ������Ʈ���� interatableObject ������Ʈ�� �ҷ��ͼ�
 * 3. delegate ������ �ڽ��� �Լ��� �ִ´�.
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
