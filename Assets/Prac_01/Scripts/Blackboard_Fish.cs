using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard_Fish : MonoBehaviour
{
    public float eatTime;
    bool eating;

    public bool Eating()
    {
        return eating;
    }
    public void SetEating(bool _eating)
    {
        eating = _eating;
    }
}
