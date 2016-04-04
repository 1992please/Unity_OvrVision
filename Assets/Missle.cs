using UnityEngine;
using System.Collections;

public class Missle : MonoBehaviour
{

    [SerializeField]
    private float RotationSpeed = 2;

    [SerializeField]
    private float MovementSpeed = 2;

    private Transform myTarget;
    // Use this for initialization
    public void StartMissle(Transform target)
    {
        myTarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        TraceTarget();
    }

    void TraceTarget()
    {
        if (myTarget)
        {
            Vector3 targetDir = myTarget.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * RotationSpeed);
        }
        transform.position += Time.deltaTime * MovementSpeed * transform.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}