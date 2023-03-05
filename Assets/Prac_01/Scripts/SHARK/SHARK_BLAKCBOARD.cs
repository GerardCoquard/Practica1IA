using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHARK_BLAKCBOARD : MonoBehaviour
{
    public float fishDetectableRadius;// within this radius worms are detected
    public float fishReachedRadius;    // at this distace worm is eatable
    public float boatDetectedRadius;
    public float homeReachedRadius;
    public float timeToEat;
    public float soundDetectableRadius;
    public float soundReachedRadius;
    public float maxNormalAcceleration;
    public float maxNormalSpeed;

    public GameObject attractor;     // hen wanders around this point
    public GameObject home;

    void Awake()
    {
        
    }

    private void Update()
    {
        //Gizmos.DrawWireSphere(home.transform.position, soundDetectableRadius);
    }
}
