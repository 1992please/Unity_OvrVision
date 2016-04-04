using UnityEngine;
using System.Collections;



public class TestTrigger : MonoBehaviour {
    //[ShowOnly]
    [ShowOnly]
    public bool test = false;

    [Header("Stuff to set and clear")]
    public GameObject[] ObjectsSetOnBuild;
    public GameObject[] ObjectsNotSetOnBuild;

    // put custome stuff here
    //public GameObject[] CustomeObjects;

    [ContextMenu("Test Version")]
    void TestVersion ()
    {
        foreach (GameObject obj in ObjectsSetOnBuild)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in ObjectsNotSetOnBuild)
        {
            obj.SetActive(true);
        }
    }

    [ContextMenu("Build Version")]
    void BuildVersion()
    {
        foreach (GameObject obj in ObjectsSetOnBuild)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in ObjectsNotSetOnBuild)
        {
            obj.SetActive(false);
        }
    }
}
