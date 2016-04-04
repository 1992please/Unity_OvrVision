using UnityEngine;
using System.Collections;

public class CameraTestControl : MonoBehaviour
{
    public float movementSpeed = 1;

    public bool bGlobalUp = false;

    [Header("Mouse Attributes")]
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public Quaternion MyRot;


    private float UpValue = 0;
    void Start()
    {
        MyRot = transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKey(KeyCode.E))
        {
            UpValue = Mathf.Lerp(UpValue, 1, 5 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            UpValue = Mathf.Lerp(UpValue, -1, 5 * Time.deltaTime);
        }
        else
        {
            UpValue = 0;
        }
        transform.position += (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal") + (bGlobalUp ? Vector3.up : transform.up) * UpValue) * movementSpeed * Time.deltaTime;
        LookRotation();
    }

    public void LookRotation()
    {
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        MyRot *= Quaternion.Euler(-xRot, yRot, 0f);
        MyRot = ZeroZAxies(MyRot);

        if (clampVerticalRotation)
            MyRot = ClampRotationAroundXAxis(MyRot);

        if (smooth)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, MyRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            transform.rotation = MyRot;
        }
    }


    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    Quaternion ZeroZAxies(Quaternion q)
    {
        Vector3 qV = q.eulerAngles;
        qV.z = 0;
        q = Quaternion.Euler(qV);

        return q;
    }
}
