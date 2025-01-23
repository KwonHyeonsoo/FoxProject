using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

/*
 * 
 * 순서
 * 1. 퍼즐 기믹만 감지하는 ray를 쏜다(매우 짧은 거리에만)
 * 2. ray는 오브젝트 1개만 검출
 * 3. 검출된 오브젝트의 각도 정도를 얻어서 정방향일때만 인식하도록 한다.
 * 4. 위의 조건을 모두 통과하면 오브젝트에 "잡기"라는 ui 텍스트를 띄운다.
 * 5. 오브젝트를 잡으면 밀거나 당길 수 있다. 끝
 */
public class PlayerObjectMove : MonoBehaviour
{
    public LayerMask mask;
    [Range(0, 360)]
    public float viewAngle;
    public TextMeshProUGUI holdText;

    public float rotationAngle = 5f;
    public float rotationInterTime = 0.1f;
    private IEnumerator rotateCoroutine;
    private bool isRotate = false;

    [SerializeField]private bool isAbleHold;
    private bool isHold;

    private GameObject holdingObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CaptureObjectLoop(0.02f));
        isAbleHold = false;
        isHold = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRotate && !isAbleHold) StopCoroutine(rotateCoroutine);
    }

    IEnumerator CaptureObjectLoop(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            CaptureObject();
        }
    }
    
    public void RotateObject(InputAction.CallbackContext context)
    {
        if (!isAbleHold) return;

        if (context.performed)
        {
            rotateCoroutine = RotateObjectPerSecond(rotationInterTime, context.ReadValue<float>() > 0, rotationAngle, holdingObject);
            StartCoroutine(rotateCoroutine);
            isRotate = true;
        }
        else if (isRotate&& context.canceled)
        {
            StopCoroutine(rotateCoroutine);
            isRotate= false;
        }
    }

    IEnumerator RotateObjectPerSecond(float time, bool right, float angle, GameObject obj)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (right)
            {
                obj.transform.rotation *= Quaternion.Euler(0, angle, 0);
            }
            else
            {
                obj.transform.rotation *= Quaternion.Euler(0, -angle, 0);
            }
        }
    }
    void CaptureObject()
    {
        if (isHold) return;

        Ray ray = new Ray();
        RaycastHit hit;

        ray.origin = transform.position;
        ray.direction = transform.forward;

        holdText.gameObject.SetActive(false);
        isAbleHold = false;

        if (Physics.Raycast(ray, out hit, 10f, mask) && hit.collider.gameObject.GetComponent<LightController>())
        {
            if (hit.collider.gameObject.GetComponent<LightController>().getLightType() == light_type.reflector)
            {
                //Debug.Log(Vector3.Angle(transform.forward, hit.normal));
                if (Vector3.Angle(transform.forward, transform.position - hit.transform.position) > viewAngle)
                {
                    Debug.Log("ok");
                    holdingObject = hit.collider.transform.gameObject;
                    holdText.gameObject.SetActive(true);
                    isAbleHold = true;
                }
            }
        }
    }

    public GameObject decal;
    public void Hold(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            if (!isAbleHold) return;

            isHold = !isHold;

            if (isHold)
            {
                holdingObject.SetActive(false);
                holdText.text = "Holdingggg";
                holdText.gameObject.SetActive(true);
            }
            else
            {
                holdingObject.SetActive(true);

                holdingObject.transform.position =
                    new Vector3(decal.transform.position.x, holdingObject.transform.position.y, decal.transform.position.z);
                holdingObject.transform.rotation = //quaternion.identity;
                    transform.rotation;
                //decal 
                holdText.text = "Hold";
                holdText.gameObject.SetActive(false);
            }
        }
    }

}
