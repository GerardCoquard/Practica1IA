using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_FishHungry", menuName = "Finite State Machines/FSM_FishHungry", order = 1)]
public class FSM_FishHungry : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    FlockingAround flockingAround;
    Blackboard_Fish_Global blackboard_global;
    float elpasedTime;
    public GameObject food;
    public bool eating;
    public bool hungry;
    public bool wandering;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        flockingAround = GetComponent<FlockingAround>();
        blackboard_global = FindObjectOfType<Blackboard_Fish_Global>();
        blackboard_global.AddVoidHungry(this);
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */
        State WanderingAround = new State("Wandering Around",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.homeAttractor; elpasedTime = 0; wandering = true; blackboard_global.SetAllWandering(); }, // write on enter logic inside {}
            () => { elpasedTime += Time.deltaTime; }, // write in state logic inside {}
            () => { flockingAround.enabled = false; wandering = false; }  // write on exit logic inisde {}
        );


        State ReachingFood = new State("Reaching Food",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.foodAttractor; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { flockingAround.enabled = false; }  // write on exit logic inisde {}
        );


        State Eating = new State("Eating",
            () => { elpasedTime = 0; eating = true; }, // write on enter logic inside {}
            () => { elpasedTime += Time.deltaTime; }, // write in state logic inside {}
            () => { eating = false; }  // write on exit logic inisde {}
        );
        State WaitingForAllAte = new State("Waiting For All Ate",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { hungry = false; blackboard_global.StartHunger(); }  // write on exit logic inisde {}
        );


        State GoingHome = new State("Going Home",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.homeAttractor; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { flockingAround.enabled = false; }  // write on exit logic inisde {}
        );
        State ReachingHome = new State("Reaching Home",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.homeAttractor; elpasedTime = 0; }, // write on enter logic inside {}
            () => { elpasedTime += Time.deltaTime; }, // write in state logic inside {}
            () => { flockingAround.enabled = false; }  // write on exit logic inisde {}
        );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
        Transition WanderToReaching = new Transition("WanderToReaching",
           () => { return hungry; }, // write the condition checkeing code in {}
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        Transition ReachingToWander = new Transition("ReachingToWander",
            () => { return wandering; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition ReachToEat = new Transition("ReachToEat",
           () => { return elpasedTime >= blackboard_global.timeToStopReachHome; }, // write the condition checkeing code in {}
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        Transition EatToWait = new Transition("EatToWait",
            () => { return elpasedTime >= blackboard_global.eatTime; }, // write the condition checkeing code in {}
            () => {
                if (food != null)
                {
                    food.SetActive(false);
                    food = null;
                }
            }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition WaitToWander = new Transition("WaitToWander",
            () => { return blackboard_global.AllEated(); }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition ReachingToHome = new Transition("ReachingToHome",
            () => {
                food = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "FOOD", blackboard_global.eatRadius);
                if (food != null)
                {
                    if (food.transform.parent.tag != "BOID")
                    {
                        food.transform.SetParent(transform);
                        return true;
                    }
                }
                food = null;
                return false;
            }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition HomeToReach = new Transition("HomeToReach",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard_global.homeAttractor) < blackboard_global.homeCloseDistance; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(WanderingAround,ReachingFood,ReachingHome,Eating, WaitingForAllAte, GoingHome);

        AddTransition(WanderingAround, WanderToReaching, ReachingFood);
        AddTransition(ReachingFood, ReachingToHome, GoingHome);
        AddTransition(ReachingFood, ReachingToWander, WanderingAround);
        AddTransition(GoingHome, HomeToReach, ReachingHome);
        AddTransition(ReachingHome, ReachToEat, Eating);
        AddTransition(Eating, EatToWait, WaitingForAllAte);
        AddTransition(WaitingForAllAte, WaitToWander, WanderingAround);


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = WanderingAround;

    }
}