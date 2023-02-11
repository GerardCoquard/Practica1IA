using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHARK_BLAKCBOARD : MonoBehaviour
{
    public float fishDetectableRadius = 60; // within this radius worms are detected
    public float fishReachedRadius = 12;    // at this distace worm is eatable
    public float timeHiding = 5.0f;

    public GameObject attractor;     // hen wanders around this point
    public GameObject home;

    void Awake()
    {
        attractor = GameObject.Find("Attractor");
        home = GameObject.Find("home");
    }
}
