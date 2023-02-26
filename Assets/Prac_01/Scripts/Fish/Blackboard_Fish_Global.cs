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
    List<FSM_FishHungry> voidsHungry = new List<FSM_FishHungry>();
    public float hungerCooldown;
    float hungerTime;
    bool hunger;

    private void Start() {
        StartHunger();
    }
    public void AddVoidHungry(FSM_FishHungry fishHungry)
    {
        voidsHungry.Add(fishHungry);
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
        foreach (FSM_FishHungry fish in voidsHungry)
        {
            fish.hungry = true;
        }
        hunger = false;
    }
    public bool AllEated()
    {
        foreach (FSM_FishHungry fish in voidsHungry)
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
        foreach (FSM_FishHungry fish in voidsHungry)
        {
            if(!fish.wandering)
            {
                fish.wandering = true;
                fish.hungry = false;
            }
        }
    }
}
