using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rigidbody;

    private Vector3 moveVec = Vector3.zero;
    private float jumpForce;

    private float limitMinX = -80;
    private float limitMaxX = 50;
    private float eulerAngleX;
    private float eulerAngleY;
    [SerializeField] private float rotCamYAxisSpeed = 5;
    [SerializeField] private float rotCamXAxisSpeed = 3;

    [SerializeField] private Camera camera;
    [SerializeField] private float speed = 10;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = moveVec;
        jumpForce = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            jumpForce = speed;
        else
            jumpForce = rigidbody.velocity.y;

        moveVec = new Vector3(Input.GetAxisRaw("Horizontal") * speed, 
                                jumpForce,
                                Input.GetAxisRaw("Vertical") * speed);
        moveVec = transform.TransformVector(moveVec);
        moveVec.y = jumpForce;
        rigidbody.velocity = moveVec;

        CameraMove(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void CameraMove(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamXAxisSpeed;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);
        camera.transform.localRotation = Quaternion.Euler(eulerAngleX, 0, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
