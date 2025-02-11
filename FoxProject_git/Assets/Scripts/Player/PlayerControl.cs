using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using static System.Net.WebRequestMethods;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    #region Components

    private CharacterController characterCtrl;

    #endregion

    //private GameObject boss;

    //이동
    [Range(0.5f, 10f)] public float moveSpeed = 1;  //기본 달리기 속도

    Vector3 direction;  //이동 방향
    private Vector3 moveForce;  //기본 이동 * 이동 방향
    private IEnumerator raycastCoroutine;
    RaycastHit hitData;

    public Collider NormalCollider;
    public Collider StealthCollider;

    ////달리기
    ///
    //[Range(0.5f, 10f)] public float runSpeed = 2;  //달리기 속도
    [Range(0.1f, 1f)] public float stealthSpeed = 0.5f;  //달리기 속도
    private bool isStealth;
    private Vector3 camNormalHeigjt;
    private Vector3 camStealthHeigjt;
    private float currentSpeed = 1; //현재 적용할 달리기 속도
    Vector3 slopeVec;
    float height;

    //점프
    [Range(0f, 50f)] public float JumpScale;
    private float jumpForce;
    [Range(-0f, -50f)] public float gravity;
    private float jumpBuffer = 0;
    [Range(0f, 5f)] public float raymaxDistance = 1.0f;

    //카메라 회전
    [SerializeField]
    [Range(0.5f, 5f)] public float rotCamXAxisSpeed = 5; //카메라 x축 회전속도

    private float limitMinX = -90;  //카메라 x축 회전 최소 범위
    private float limitMaxX = 90;

    private float eulerAngleX;
    private float eulerAngleY;

    public Camera cam;

    int i_width;
    int i_height;

    private bool rotateLock = false;

    // Start is called before the first frame update
    void Start()
    {

        #region GetComponents
        characterCtrl = GetComponent<CharacterController>();
        Rigidbody rb;
        //회전
        i_width = Screen.width;
        i_height = Screen.height;

        #endregion

        height = characterCtrl.height * transform.localScale.y + 0.1f;
        isStealth = false;
        camNormalHeigjt = cam.transform.localPosition ;
        camStealthHeigjt = cam.transform.localPosition + new Vector3(0f,-0.5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        jumpBuffer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //캐릭터 이동
        characterCtrl.Move(moveForce * currentSpeed *      Time.deltaTime);
        //이동 방향         걷는 방향   스텔스&달리기 유무

        //중력 적용
        if (!IsCheckGrounded())
        {
            moveForce.y += gravity * Time.deltaTime;
        }
        else if (isStealth)   //미리 스텔스를 찍어놨다면 바닥에 닿자마자 스텔스 모드
        {
            currentSpeed = stealthSpeed;
            cam.transform.DOLocalMoveY(camStealthHeigjt.y, 0.1f);
            StealthCollider.enabled = true;
            NormalCollider.enabled = false;
        }
        else        
        {
            moveForce = transform.rotation * direction;  //바닥에 닿았을 때 이동 버튼 뗀거 적용

            if (jumpBuffer > 0 )//점프 버퍼
            {
                moveForce.y = JumpScale;
            }
        }
        if (IsCheckGrounded())  //이동 적용
        {
            moveForce = new Vector3(slopeVec.x, slopeVec.y, slopeVec.z);
        }

    }

    #region Character Move
    /*
     * Disabled, Waiting, Started, Performed, Canceled
     */
    public void OnMove(InputAction.CallbackContext value)
    {
        

            float directionX = value.ReadValue<Vector2>().x;
            float directionY = value.ReadValue<Vector2>().y;
            direction = new Vector3(directionX, 0, directionY).normalized;

        if (value.performed)
        {
            raycastCoroutine = RecayGround(0.1f);
            StartCoroutine(raycastCoroutine);
        }
        else if (value.canceled)
        {
            StopCoroutine(raycastCoroutine);
            slopeVec = Vector3.zero;
        }

    }
    
    IEnumerator RecayGround(float time)
    {
        while (true)
        {
            Ray r = new Ray(transform.position, Vector3.down);
            Physics.Raycast(r, out hitData, height, ~(1 << 7));
            slopeVec = Vector3.ProjectOnPlane(transform.rotation * direction, hitData.normal).normalized * moveSpeed;

            yield return new WaitForSeconds(time);
        }
    }

    public void OnStealth(InputAction.CallbackContext value)
    {
        {
            if (value.started)
            {
                isStealth = true;

                if (IsCheckGrounded()) // && currentStamina > 0)
                {
                    currentSpeed = stealthSpeed;

                    cam.transform.DOLocalMoveY(camStealthHeigjt.y, 0.1f);
                    StealthCollider.enabled = true;
                    NormalCollider.enabled = false;
                }

            }
            else if (value.canceled)
            {
               isStealth = false;
                currentSpeed = 1;
                cam.transform.DOLocalMoveY(camNormalHeigjt.y, 0.1f);
                StealthCollider.enabled = false;
                NormalCollider.enabled = true;
            }
        }
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (IsCheckGrounded() && !isStealth)
            {
                
                moveForce.y = JumpScale;
                Debug.LogWarning(moveForce +" "+ IsCheckGrounded());

            }
            else if (!IsCheckGrounded() && !isStealth)  //점프 버퍼
            { jumpBuffer = 0.05f; }
        }
    }

    #endregion
    private bool IsCheckGrounded()
    {
        // CharacterController.IsGrounded가 true라면 Raycast를 사용하지 않고 판정 종료
        if (characterCtrl.isGrounded) return true;
        // 발사하는 광선의 초기 위치와 방향
        // 약간 신체에 박혀 있는 위치로부터 발사하지 않으면 제대로 판정할 수 없을 때가 있다.
        var ray = new Ray(this.transform.position , Vector3.down);
        // 탐색 거리
        var maxDistance = height;
        // 광선 디버그 용도
        Debug.DrawRay(transform.position , Vector3.down * maxDistance, Color.red);
        // Raycast의 hit 여부로 판정
        // 지상에만 충돌로 레이어를 지정
        return Physics.Raycast(ray, maxDistance, ~(1 << 7));

    }
    #region Camera Rotate
    public void rotateOnOff(bool on)
    {
        if (on)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            rotateLock = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            rotateLock = true;
        }
        //Debug.Log("rotatteLock : " + rotateLock);
    }
    
    public void onCameraRotate(InputAction.CallbackContext value)
    {
        if (!rotateLock)
        {
            float mouseX = value.ReadValue<Vector2>().x;// - i_width / 2;
            float mouseY = value.ReadValue<Vector2>().y;// - i_height / 2;
    
            eulerAngleY = mouseX * rotCamXAxisSpeed * Time.fixedDeltaTime;
            eulerAngleX = mouseY * rotCamXAxisSpeed * Time.fixedDeltaTime * ((float)i_height / i_width);
    
            eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
    
            //camera up and down rotate
            cam.transform.rotation *= Quaternion.Euler(-eulerAngleX, 0, 0);
    
            //gameobject transfomr left and right rotate ( for move forward direction ) dho gksrmf dkseho
            transform.rotation *= Quaternion.Euler(0, eulerAngleY, 0);
    
            if (cam.transform.localEulerAngles.y > 0 || cam.transform.localEulerAngles.z > 0)
            {
                cam.transform.localRotation = Quaternion.Euler(cam.transform.localRotation.eulerAngles.x, 0, 0);
            }
    
    
        }
    
    }
    
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
    
        return Mathf.Clamp(angle, min, max);
    }
    
    #endregion
}
