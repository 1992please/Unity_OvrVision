using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RocketLauncher : MonoBehaviour
{

    [SerializeField]
    private GameObject Missle;

    [SerializeField]
    private Image CrossHair;

    [SerializeField]
    private float trackingDistance = 50;

    const string target = "MissileTarget";
    private AudioSource m_Audio;

    void Start()
    {
        m_Audio = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 1, transform.forward, out hit, trackingDistance))
        {
            CrossHair.color = Color.blue;
            m_Audio.enabled = false;
            if (hit.transform.CompareTag(target))
            {
                CrossHair.color = Color.red;
                m_Audio.enabled = false;
            }
        }

        Debug.DrawRay(transform.position, transform.forward * trackingDistance, Color.yellow);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.SphereCast(transform.position, 1, transform.forward, out hit, trackingDistance))
            {
                CrossHair.color = Color.red;
                if (hit.transform.CompareTag(target))
                {
                    SpawnMissle(hit.transform);
                    return;
                }
            }
            SpawnMissle(null);
        }

    }

    void SpawnMissle(Transform myTarget)
    {
        GameObject NewMissle = Instantiate(Missle, transform.position, transform.rotation) as GameObject;
        Missle MissleScript = NewMissle.GetComponent<Missle>();
        MissleScript.StartMissle(myTarget);
    }
}
