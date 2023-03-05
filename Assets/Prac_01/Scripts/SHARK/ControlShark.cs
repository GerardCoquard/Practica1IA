using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlShark : MonoBehaviour
{
    private Camera cam;
    private GameObject wormPrefab;
    private GameObject chickPrefab;
    private GameObject dummy;
    public GameObject boatPrefab;
    private GameObject boat;
    private float boatTimeToDisappear = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        //dummy = new GameObject("dummy");
        //dummy.tag = "DUMMY";
        cam = Camera.main;
        wormPrefab = Resources.Load<GameObject>("WORM");
        chickPrefab = Resources.Load<GameObject>("CHICK");

        dummy = new GameObject("dummy");
        dummy.tag = "RED_TAG";
        dummy.SetActive(false);

        boat = Instantiate(boatPrefab);
        boat.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            dummy.transform.position = position;
            dummy.SetActive(true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            boat.transform.position = position;
            boat.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(DisableBoat());
        }

        if (Input.GetMouseButtonDown(2))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            //GameObject chick = GameObject.Instantiate(chickPrefab);
            //chick.transform.position = position;
            //chick.transform.Rotate(0, 0, Random.value * 360);
        }
    }

    IEnumerator DisableBoat()
    {
        yield return new WaitForSeconds(boatTimeToDisappear);

        boat.SetActive(false);
    }
}
