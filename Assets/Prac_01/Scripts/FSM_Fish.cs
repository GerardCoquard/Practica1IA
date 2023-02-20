using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Fish", menuName = "Finite State Machines/FSM_Fish", order = 1)]
public class FSM_Fish : FiniteStateMachine
{
    FlockingAround flockingAround;
    Flee flee;
    SteeringContext context;
    Blackboard_Fish_Global blackboard_global;
    float elpasedTime;
    public GameObject food;
    public bool eating;
    public bool hungry;
    public bool wandering;
    IState lastState;

    public override void OnEnter()
    {
        flockingAround = GetComponent<FlockingAround>();
        flee = GetComponent<Flee>();
        context = GetComponent<SteeringContext>();
        blackboard_global = FindObjectOfType<Blackboard_Fish_Global>();
        flee.target = blackboard_global.shark;
        blackboard_global.AddVoid(this);
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
            () => { elpasedTime+=Time.deltaTime; }, // write in state logic inside {}
            () => { flockingAround.enabled = false; wandering = false; }  // write on exit logic inisde {}
        );

        State ReachingFood = new State("Reaching Food",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.foodAttractor; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { flockingAround.enabled = false; }  // write on exit logic inisde {}
        );

        State GoingHome = new State("Going Home",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.homeAttractor; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { flockingAround.enabled = false; }  // write on exit logic inisde {}
        );
        State ReachingHome = new State("Reaching Home",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.homeAttractor; elpasedTime = 0;}, // write on enter logic inside {}
            () => { elpasedTime+=Time.deltaTime; }, // write in state logic inside {}
            () => { flockingAround.enabled = false; }  // write on exit logic inisde {}
        );

        State Eating = new State("Eating",
            () => { elpasedTime = 0; eating = true; }, // write on enter logic inside {}
            () => { elpasedTime+=Time.deltaTime; }, // write in state logic inside {}
            () => { eating = false; }  // write on exit logic inisde {}
        );
        State WaitingForAllAte = new State("Waiting For All Ate",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { hungry = false; blackboard_global.StartHunger(); }  // write on exit logic inisde {}
        );
        State Fleeing = new State("Fleeing",
            () => { flee.enabled = true; elpasedTime = 0; context.maxSpeed*=blackboard_global.fleeSpeedMultiplier;}, // write on enter logic inside {}
            () => { elpasedTime+=Time.deltaTime; }, // write in state logic inside {}
            () => { flee.enabled = false; lastState = previousState; context.maxSpeed/=blackboard_global.fleeSpeedMultiplier;}  // write on exit logic inisde {}
        );
        State PreviousState = new State("Previous",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}
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
        Transition WanderToFlee = new Transition("WanderToFlee",
            () => { return SensingUtils.DistanceToTarget(gameObject,blackboard_global.shark) < blackboard_global.fleeDistanceTrigger; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition ReachingToHome = new Transition("ReachingToHome",
            () => { food = SensingUtils.FindRandomInstanceWithinRadius(gameObject,"FOOD",blackboard_global.eatRadius);
                    if(food!=null)
                    {
                        if(food.transform.parent.tag != "BOID")
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
        Transition ReachingToWander = new Transition("ReachingToWander",
            () => { return wandering; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition ReachingToFlee = new Transition("ReachingToFlee",
            () => { return SensingUtils.DistanceToTarget(gameObject,blackboard_global.shark) < blackboard_global.fleeDistanceTrigger; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition HomeToReach = new Transition("HomeToReach",
            () => { return SensingUtils.DistanceToTarget(gameObject,blackboard_global.homeAttractor) < blackboard_global.homeCloseDistance; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition HomeToFlee = new Transition("HomeToFlee",
            () => { return SensingUtils.DistanceToTarget(gameObject,blackboard_global.shark) < blackboard_global.fleeDistanceTrigger; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition ReachToEat = new Transition("ReachToEat",
            () => { return elpasedTime >= blackboard_global.timeToStopReachHome; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition ReachToFlee = new Transition("ReachToFlee",
            () => { return SensingUtils.DistanceToTarget(gameObject,blackboard_global.shark) < blackboard_global.fleeDistanceTrigger; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition EatToWait = new Transition("EatToWait",
            () => { return elpasedTime >= blackboard_global.eatTime; }, // write the condition checkeing code in {}
            () => { if(food!=null)
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
        Transition FleeToPrevious = new Transition("FleeToPrevious",
            () => { return elpasedTime >= blackboard_global.fleeTime; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        Transition PreviousToWander = new Transition("PreviousToWander",
            () => { return lastState == WanderingAround; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition PreviousToReach = new Transition("PreviousToReach",
            () => { return lastState == ReachingFood; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition PreviousToHome = new Transition("PreviousToHome",
            () => { return lastState == GoingHome; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        



        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);
        

        AddTransition(sourceState, transition, destinationState);

         */ 


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

        AddStates(WanderingAround,ReachingFood,GoingHome,ReachingHome,Eating,WaitingForAllAte,Fleeing,PreviousState);

        AddTransition(WanderingAround,WanderToReaching,ReachingFood);
        AddTransition(WanderingAround,WanderToFlee,Fleeing);
        AddTransition(ReachingFood,ReachingToHome,GoingHome);
        AddTransition(ReachingFood,ReachingToWander,WanderingAround);
        AddTransition(ReachingFood,ReachingToFlee,Fleeing);
        AddTransition(GoingHome,HomeToReach,ReachingHome);
        AddTransition(GoingHome,HomeToFlee,Fleeing);
        AddTransition(ReachingHome,ReachToEat,Eating);
        AddTransition(Eating,EatToWait,WaitingForAllAte);
        AddTransition(WaitingForAllAte,WaitToWander,WanderingAround);
        AddTransition(Fleeing,FleeToPrevious,PreviousState);
        AddTransition(PreviousState,PreviousToWander,WanderingAround);
        AddTransition(PreviousState,PreviousToReach,ReachingFood);
        AddTransition(PreviousState,PreviousToHome,GoingHome);

        initialState = WanderingAround;
    }
}
