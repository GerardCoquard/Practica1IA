using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard_Fish_Global : MonoBehaviour
{
    public GameObject homeAttractor;
    public GameObject foodAttractor;
    public GameObject shark;
    public float eatRadius;
    public float eatTime;
    public float homeCloseDistance;
    public float timeToStopReachHome;
    public float fleeTime;
    public float fleeDistanceTrigger;
    public float fleeSpeedMultiplier;
    public List<FSM_Fish> voids = new List<FSM_Fish>();
    public float hungerCooldown;
    float hungerTime;
    bool hunger;

    private void Start() {
        StartHunger();
    }
    public void AddVoid(FSM_Fish fishHungry)
    {
        voids.Add(fishHungry);
    }
    public void RemoveVoid(FSM_Fish fishHungry)
    {
        voids.Remove(fishHungry);
    }
    public void StartHunger()
    {
        if(!hunger) StartCoroutine(Hunger());    
    }
    IEnumerator Hunger()
    {
        hunger = true;
        hungerTime = 0;
        while(hungerTime < hungerCooldown)
        {
            hungerTime += Time.deltaTime;
            yield return null;
        }
        foreach (FSM_Fish fish in voids)
        {
            fish.hungry = true;
        }
        hunger = false;
    }
    public bool AllEated()
    {
        foreach (FSM_Fish fish in voids)
        {
            if(fish.food!=null)
            {
                return false;
            }
        }
        return true;
    }
    public void SetAllWandering()
    {
        foreach (FSM_Fish fish in voids)
        {
            if(!fish.wandering)
            {
                fish.wandering = true;
                fish.hungry = false;
            }
        }
    }
}
