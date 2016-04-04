using UnityEngine;
using System.Collections;

public class PlanDestroyMine : MonoBehaviour
{
    public Rigidbody plane;
    public float Strength;
    public Transform Boom;
    public Transform Dead;
    public GameObject WholePlane;
    public float minPitch = 1;
    public float maxPitch = 2;
    void OnCollisionEnter(Collision collision)
    {
    //    print("lOL");
      //  if (collision.relativeVelocity.magnitude > Strength)
      //  {
            Instantiate(Boom, plane.position, plane.rotation);
            Instantiate(Dead, plane.position, plane.rotation);
            Destroy(WholePlane, 0);
     //   }
    }
    void FixedUpdate()
    {
        var currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        GetComponent<AudioSource>().pitch = minPitch + ((currentSpeed) / maxPitch);

    }
}
