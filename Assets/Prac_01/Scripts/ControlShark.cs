using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlShark : MonoBehaviour
{
    private Camera cam;
    private GameObject wormPrefab;
    private GameObject chickPrefab;
    //private GameObject dummy;

    // Start is called before the first frame update
    void Start()
    {
        //dummy = new GameObject("dummy");
        //dummy.tag = "DUMMY";
        cam = Camera.main;
        wormPrefab = Resources.Load<GameObject>("WORM");
        chickPrefab = Resources.Load<GameObject>("CHICK");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            //dummy.transform.position = position;

            GameObject dummy = new GameObject("dummy");
            dummy.tag = "DUMMY";
            dummy.transform.position = position;

            Destroy(dummy, 3.0f);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            //GameObject theDummy = SensingUtils.FindInstanceWithinRadius(gameObject, "DUMMY", 500);
            //if (theDummy != null)
            //    Destroy(theDummy);
        }

        if (Input.GetMouseButtonDown(2))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            GameObject chick = GameObject.Instantiate(chickPrefab);
            chick.transform.position = position;
            chick.transform.Rotate(0, 0, Random.value * 360);
        }
    }
}
