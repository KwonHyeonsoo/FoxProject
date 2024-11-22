using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    #region Components

    private CharacterController characterCtrl;

    #endregion

    private GameObject boss;

    //�̵�
    [Range(0.5f, 10f)] public float moveSpeed = 1;  //�⺻ �޸��� �ӵ�

    Vector3 direction;  //�̵� ����
    private Vector3 moveForce;  //�⺻ �̵� * �̵� ����

    //�޸���
    [Range(0.5f, 10f)] public float runSpeed = 2;  //�޸��� �ӵ�
    [Range(0.1f, 1f)] public float stealthSpeed = 0.5f;  //�޸��� �ӵ�

    private float currentSpeed = 1; //���� ������ �޸��� �ӵ�

    //����
    [Range(0f, 50f)] public float JumpScale;
    [Range(-0f, -50f)] public float gravity;

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

        //ȸ��
        i_width = Screen.width;
        i_height = Screen.height;

        #endregion

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //ĳ���� �̵�
        characterCtrl.Move(transform.rotation * moveForce * currentSpeed * Time.deltaTime);

        //�߷� ����
        if (!characterCtrl.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }

    }

    #region Character Move
/*
 * Disabled, Waiting, Started, Performed, Canceled
 */
public void OnMove(InputAction.CallbackContext value)
{
    if (characterCtrl.isGrounded)
    {
        float directionX = value.ReadValue<Vector2>().x;
        float directionY = value.ReadValue<Vector2>().y;
        direction = new Vector3(directionX, direction.y, directionY).normalized * moveSpeed;

        moveForce = direction;
    }

}

public void OnRun(InputAction.CallbackContext value)
{
    if (characterCtrl.isGrounded ) // && currentStamina > 0)
    {
        currentSpeed = 1 + (runSpeed * value.ReadValue<float>());

    }
    if (value.canceled)
    {

    }

}

public void OnStealth(InputAction.CallbackContext value)
{
    if (characterCtrl.isGrounded) // && currentStamina > 0)
    {
        if (value.started)
        {
            currentSpeed = stealthSpeed;
            cam.transform.position += new Vector3(0, -0.5f, 0);

        }
        else if (value.canceled)
        {
            currentSpeed = 1;
            cam.transform.position += new Vector3(0, +0.5f, 0);


        }
    }

}

public void OnJump(InputAction.CallbackContext value)
{
    if (characterCtrl.isGrounded)
    {
        moveForce.y = JumpScale;

    }
}

#endregion

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
    Debug.Log("rotatteLock : " + rotateLock);
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
