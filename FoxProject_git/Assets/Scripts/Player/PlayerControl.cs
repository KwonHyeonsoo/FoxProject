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

    //�̵�
    [Range(0.5f, 10f)] public float moveSpeed = 1;  //�⺻ �޸��� �ӵ�

    Vector3 direction;  //�̵� ����
    private Vector3 moveForce;  //�⺻ �̵� * �̵� ����
    private IEnumerator raycastCoroutine;
    RaycastHit hitData;

    public Collider NormalCollider;
    public Collider StealthCollider;

    ////�޸���
    ///
    //[Range(0.5f, 10f)] public float runSpeed = 2;  //�޸��� �ӵ�
    [Range(0.1f, 1f)] public float stealthSpeed = 0.5f;  //�޸��� �ӵ�
    private bool isStealth;
    private Vector3 camNormalHeigjt;
    private Vector3 camStealthHeigjt;
    private float currentSpeed = 1; //���� ������ �޸��� �ӵ�
    Vector3 slopeVec;
    float height;

    //����
    [Range(0f, 50f)] public float JumpScale;
    private float jumpForce;
    [Range(-0f, -50f)] public float gravity;
    private float jumpBuffer = 0;
    [Range(0f, 5f)] public float raymaxDistance = 1.0f;

    //ī�޶� ȸ��
    [SerializeField]
    [Range(0.5f, 5f)] public float rotCamXAxisSpeed = 5; //ī�޶� x�� ȸ���ӵ�

    private float limitMinX = -90;  //ī�޶� x�� ȸ�� �ּ� ����
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
        //ȸ��
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
        //ĳ���� �̵�
        characterCtrl.Move(moveForce * currentSpeed *      Time.deltaTime);
        //�̵� ����         �ȴ� ����   ���ڽ�&�޸��� ����

        //�߷� ����
        if (!IsCheckGrounded())
        {
            moveForce.y += gravity * Time.deltaTime;
        }
        else if (isStealth)   //�̸� ���ڽ��� �����ٸ� �ٴڿ� ���ڸ��� ���ڽ� ���
        {
            currentSpeed = stealthSpeed;
            cam.transform.DOLocalMoveY(camStealthHeigjt.y, 0.1f);
            StealthCollider.enabled = true;
            NormalCollider.enabled = false;
        }
        else        
        {
            moveForce = transform.rotation * direction;  //�ٴڿ� ����� �� �̵� ��ư ���� ����

            if (jumpBuffer > 0 )//���� ����
            {
                moveForce.y = JumpScale;
            }
        }
        if (IsCheckGrounded())  //�̵� ����
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
            else if (!IsCheckGrounded() && !isStealth)  //���� ����
            { jumpBuffer = 0.05f; }
        }
    }

    #endregion
    private bool IsCheckGrounded()
    {
        // CharacterController.IsGrounded�� true��� Raycast�� ������� �ʰ� ���� ����
        if (characterCtrl.isGrounded) return true;
        // �߻��ϴ� ������ �ʱ� ��ġ�� ����
        // �ణ ��ü�� ���� �ִ� ��ġ�κ��� �߻����� ������ ����� ������ �� ���� ���� �ִ�.
        var ray = new Ray(this.transform.position , Vector3.down);
        // Ž�� �Ÿ�
        var maxDistance = height;
        // ���� ����� �뵵
        Debug.DrawRay(transform.position , Vector3.down * maxDistance, Color.red);
        // Raycast�� hit ���η� ����
        // ���󿡸� �浹�� ���̾ ����
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
