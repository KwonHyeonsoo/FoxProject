//using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCC_CameraRotate : MonoBehaviour
{
    public SCC_Inputs inputs;

    Vector2 cameraDelta;

    [SerializeField]
    [Range(0.5f, 5f)] public float rotCamXAxisSpeed = 5; //카메라 x축 회전속도

    private float limitMinX = -90;  //카메라 x축 회전 최소 범위
    private float limitMaxX = 90;

    private float eulerAngleX;
    private float eulerAngleY;

    int i_width;
    int i_height;

    private bool rotateLock = false;
    void Start()
    {
        inputs = GetComponentInParent<SCC_InputProcessor>().inputs;
        i_width = Screen.width;
        i_height = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        cameraDelta = inputs.cameraVector;
        //Debug.Log(cameraDelta);
        onCameraRotate(cameraDelta);
    }

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

    public void onCameraRotate(Vector2 value)
    {
        if (!rotateLock)
        {
            float mouseX = value.x;// - i_width / 2;
            float mouseY = 0;// - i_height / 2;

            eulerAngleY = mouseX * rotCamXAxisSpeed * Time.deltaTime;
            eulerAngleX = mouseY * rotCamXAxisSpeed * Time.deltaTime * ((float)i_height / i_width);

            //Debug.Log(eulerAngleX +" "+ eulerAngleY);
            eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

            //Debug.Log(-mouseX);
            //camera up and down rotate
            transform.rotation *= Quaternion.Euler(-eulerAngleX, eulerAngleY, 0);

            //gameobject transfomr left and right rotate ( for move forward direction ) dho gksrmf dkseho
            //transform.rotation *= Quaternion.Euler(0, eulerAngleY, 0);

            //if (transform.localEulerAngles.y > 0 || transform.localEulerAngles.z > 0)
            //{
            //    transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, 0, 0);
            //}


        }

    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

}
