using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHARK_BLAKCBOARD : MonoBehaviour
{
    public float fishDetectableRadius = 60; // within this radius worms are detected
    public float fishReachedRadius = 12;    // at this distace worm is eatable
    public float timeHiding = 5.0f;
    public float soundDetectableRadius = 300;
    public float soundReachedRadius = 20;

    public GameObject attractor;     // hen wanders around this point
    public GameObject home;

    void Awake()
    {
        attractor = GameObject.Find("Attractor");
        home = GameObject.Find("home");
    }

    private void Update()
    {
        Gizmos.DrawWireSphere(home.transform.position, soundDetectableRadius);
    }
}
