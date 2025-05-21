using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using DG.Tweening;
using System.ComponentModel;

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
    public GameObject Cam;
    public LayerMask mask;
    [Range(0, 360)]
    public float viewAngle;
    //public TextMeshProUGUI holdText;

    public float rotationAngle = 5f;
    public float rotationInterTime = 0.1f;
    private IEnumerator rotateCoroutine;
    private bool isRotate = false;

    private bool isAbleHold;
    private bool isHold;

    
    private bool isInteractable;

    public Ease ease = Ease.InQuint;
    [ReadOnly][SerializeField]private GameObject holdingObject;

    //decal
    public GameObject decal;
    PlayerMoveEnable PlayerMoveEnable;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CaptureObjectLoop(0.02f));
        isAbleHold = false;
        isHold = false;
        isInteractable = false;
        decal.SetActive(false);
        //holdText?.gameObject.SetActive(false);
        PlayerMoveEnable = decal.GetComponent<PlayerMoveEnable>();
    }

    private void OnEnable()
    {
        StartCoroutine(CaptureObjectLoop(0.02f));
        isAbleHold = false;
        isHold = false;
        isInteractable = false;
        decal.SetActive(false);
        //holdText?.gameObject.SetActive(false);
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
        if (Managers.gameManager.IsInputLock) return;
        if (!isAbleHold) return;

        if (context.performed)
        {
            
            Vector2 v = context.ReadValue<Vector2>();
            //Debug.Log(v);
            if (v.y > 0)
            {
                holdingObject.transform.rotation *= Quaternion.Euler(rotationAngle, 0, 0);
            }
            else if (v.y < 0)
            {
                holdingObject.transform.rotation *= Quaternion.Euler(-rotationAngle, 0, 0);
            }
            else
            {
                if (!isRotate)
                {
                    rotateCoroutine = RotateObjectPerSecond(rotationInterTime, context.ReadValue<Vector2>(), rotationAngle, holdingObject);
                    StartCoroutine(rotateCoroutine);
                    isRotate = true;
                }
            }
            
            
        }
        else if (isRotate&& context.canceled)
        {
            StopCoroutine(rotateCoroutine);
            isRotate= false;
        }
    }

    IEnumerator RotateObjectPerSecond(float time, Vector2 v, float angle, GameObject obj)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
           // Debug.Log(v);

            if (v.x > 0)
            {
                obj.transform.rotation *= Quaternion.Euler(0, angle, 0);
            }
            else if (v.x < 0)
            {
                obj.transform.rotation *= Quaternion.Euler(0, -angle, 0);
            }

        }
    }
    void CaptureObject()
    {
        if (Managers.gameManager.IsInputLock) return;
        if (isHold) return;

        Ray ray = new Ray();
        RaycastHit hit;

        ray.origin = Cam.transform.position;
        ray.direction = Cam.transform.forward;
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.3f);

        Managers.UI_manager.UI_holdTextControl(false, UI_Manager.UI_hold_status.DEFAULT);
        isAbleHold = false;
        isInteractable = false;


        //if (Physics.Raycast(ray, out hit, 10f, mask) )
        if (Physics.SphereCast(ray.origin,0.1f, ray.direction * 10, out hit, 10f, mask))
        {
            if (hit.collider.gameObject.GetComponent<LightController>() &&
                hit.collider.gameObject.GetComponent<LightController>().getLightType() == light_type.reflector)
            {
                //if (Vector3.Angle(transform.forward, transform.position - hit.transform.position) > viewAngle)
                {
                    holdingObject = hit.collider.transform.gameObject;
                    Managers.UI_manager.UI_holdTextControl(true, UI_Manager.UI_hold_status.HOLD);
                    isAbleHold = true;
                }
            }
            else if(hit.collider.gameObject.tag == "Interactable")
            {
                holdingObject = hit.collider.transform.gameObject;
                isInteractable = true;
                Managers.UI_manager.UI_holdTextControl(true, UI_Manager.UI_hold_status.INTERACT);

            }
        }
        //Debug.Log("isAbleHold" + isAbleHold + "isInteractable" + isInteractable);

    }


    public void Hold(InputAction.CallbackContext context)
    {
        if (Managers.gameManager.IsInputLock) return;

        if (context.performed)
        {
            //물체를 움직일때
            if (isAbleHold)
            {

                isHold = !isHold;

                Managers.storyManager.InvokeINTERACTION();
                if (isHold)
                {
                    holdingObject.SetActive(false);
                    //holdText.text = "Holdingggg";
                    //holdText?.gameObject.SetActive(true);
                    Managers.UI_manager.UI_holdTextControl(true, UI_Manager.UI_hold_status.HOLDING);
                    decal.SetActive(true);
                }
                else
                {
                   // Debug.Log(PlayerMoveEnable.getUnholdEnable());
                    if (PlayerMoveEnable.getUnholdEnable())
                    {

                        Vector3 trans;
                        trans =
                        holdingObject.transform.position =
                            new Vector3(decal.transform.position.x, transform.position.y + 0.5f, decal.transform.position.z);
                        holdingObject.transform.rotation = //quaternion.identity;
                            transform.rotation;

                        holdingObject.transform.DOMoveY(trans.y - 0.2f, 0.1f).SetEase(ease);

                        holdingObject.SetActive(true);

                        Managers.UI_manager.UI_holdTextControl(false, UI_Manager.UI_hold_status.DEFAULT);
                        decal.SetActive(false);

                        Managers.soundManager.PlaySoundOneShot(SoundManager.OneShotSound._reflctor);

                    }
                    else
                    {
                        isHold = !isHold;
                    }
                }
            }
            //차에 탑승
            else if (isInteractable)
            {
                Managers.storyManager.InvokeRIDE();
                isHold = false;
                isAbleHold = false;
                isInteractable = false;
                Managers.UI_manager.UI_holdTextControl(false, UI_Manager.UI_hold_status.DEFAULT);
                var com = holdingObject.GetComponent<InteractableObject>();
                com.Invoke(gameObject);
                //InputManager.SwitchInput();
            }
        }
    }

}
