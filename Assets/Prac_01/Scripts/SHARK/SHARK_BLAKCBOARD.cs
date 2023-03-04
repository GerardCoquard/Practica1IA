using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHARK_BLAKCBOARD : MonoBehaviour
{
    public float fishDetectableRadius = 5; // within this radius worms are detected
    public float fishReachedRadius = 1;    // at this distace worm is eatable
    public float boatDetectedRadius = 500;
    public float homeReachedRadius = 5;
    public float timeToEat = 3.0f;
    public float soundDetectableRadius = 300;
    public float soundReachedRadius = 20;
    public float maxNormalAcceleration = 40;
    public float maxNormalSpeed= 10;

    public GameObject attractor;     // hen wanders around this point
    public GameObject home;

    void Awake()
    {
        attractor = GameObject.Find("Attractor");
        home = GameObject.Find("Cave");
    }

    private void Update()
    {
        //Gizmos.DrawWireSphere(home.transform.position, soundDetectableRadius);
    }
}
