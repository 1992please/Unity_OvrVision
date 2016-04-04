using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour
{
    public Rigidbody Obj;
    public int zrotForce = 4;
    public int MaxRot = 90;
    public int MinRot = -90;
    public int rotupForce = 1;
    public float speed = 50;
    public float speedincrease = 4;
    public float speeddecrease = 1;
    public int Maxspeed = 100;
    public int Minspeed = 0;
    public int takeoffspeed = 20;
    public int lift = 3;
    public int minlift = 0;
    public bool hit = false;
    void Start()
    {

        InvokeRepeating("Speed", .1f, .1f);
    }

    void Speed()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            Mathf.Repeat(1, Time.time);
            speed = speed + speedincrease;
        }
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Mathf.Repeat(1, Time.time);
            speed = speed - speeddecrease;
        }
    }


    void Update()
    {
        var spd = Obj.velocity.magnitude;
        Obj.GetComponent<Rigidbody>().AddRelativeForce(0, 0, -speed);
        float H = (Input.GetAxis("Horizontal")) * zrotForce;

        Obj.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, H * (spd / 100));

        float V = (Input.GetAxis("Vertical")) * rotupForce;

        Obj.GetComponent<Rigidbody>().AddRelativeTorque(V * (spd / 100), 0, 0);


        if (Maxspeed <= speed)
        {
            speed = Maxspeed;
        }
        if (Minspeed >= speed)
        {
            speed = Minspeed;
        }
        if (speed < takeoffspeed)
        {
            Obj.GetComponent<Rigidbody>().AddForce(0, minlift, 0);

        }
        if (speed > takeoffspeed)
        {
            Obj.GetComponent<Rigidbody>().AddForce(0, lift, 0);
        }
        //if (Obj.GetComponent<Rigidbody>().rotation.z > MaxRot)
        //{
        //    Obj.GetComponent<Rigidbody>().rotation.z = MaxRot;
        //}
        //if (Obj.GetComponent<Rigidbody>().rotation.z < MinRot)
        //{
        //    Obj.GetComponent<Rigidbody>().rotation.z = MinRot;
    }
}

