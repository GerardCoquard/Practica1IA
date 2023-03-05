using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Fish", menuName = "Finite State Machines/FSM_Fish", order = 1)]
public class FSM_Fish : FiniteStateMachine
{
    FlockingAround flockingAround;
    Flee flee;
    Arrive arrive;
    SteeringContext context;
    public Blackboard_Fish_Global blackboard_global;
    float elpasedTime;
    FSM_FishFather fsmFish;
    IState lastState;
    GameObject surrogateTarget;

    public override void OnEnter()
    {
        arrive = GetComponent<Arrive>();
        flockingAround = GetComponent<FlockingAround>();
        flee = GetComponent<Flee>();
        context = GetComponent<SteeringContext>();
        blackboard_global = FindObjectOfType<Blackboard_Fish_Global>();
        flee.target = blackboard_global.shark;
        fsmFish = (FSM_FishFather)GetComponent<FSMExecutor>().fsm;
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        DisableAllSteerings();
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
        surrogateTarget = transform.GetChild(0).gameObject;
        surrogateTarget.transform.SetParent(null);

        State FlockingAround = new State("Flocking Around",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.homeAttractor; elpasedTime = 0; fsmFish.flocking = true; blackboard_global.SetAllWandering(); blackboard_global.StartHunger(); }, // write on enter logic inside {}
            () => { elpasedTime += Time.deltaTime; }, // write in state logic inside {}
            () => { flockingAround.enabled = false; fsmFish.flocking = false; }  // write on exit logic inisde {}
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
        () => {
                arrive.enabled = true;
                fsmFish.homeReached = false;
                int angle = Random.Range(0,361);
                float distance = Random.Range(0f,blackboard_global.homeCloseDistance);
                surrogateTarget.transform.position = (Utils.OrientationToVector(angle).normalized * distance) + blackboard_global.homeAttractor.transform.position;
                arrive.target = surrogateTarget;
                fsmFish.reaching = true;
            }, // write on enter logic inside {}
        () => { }, // write in state logic inside {}
        () => { fsmFish.reaching = false; arrive.enabled = false; }  // write on exit logic inisde {}
        );

        State Fleeing = new State("Fleeing",
            () => { flee.enabled = true; elpasedTime = 0; context.maxSpeed *= blackboard_global.fleeSpeedMultiplier; }, // write on enter logic inside {}
            () => { elpasedTime += Time.deltaTime; }, // write in state logic inside {}
            () => { flee.enabled = false; lastState = previousState; context.maxSpeed /= blackboard_global.fleeSpeedMultiplier; }  // write on exit logic inisde {}
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
            () => { return fsmFish.hungry; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition ReachingToHome = new Transition("ReachingToHome",
            () => {
                fsmFish.food = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "FOOD", blackboard_global.eatRadius);
                if (fsmFish.food != null)
                {
                    if (fsmFish.food.transform.parent.tag != "BOID")
                    {
                        fsmFish.food.transform.SetParent(transform);
                        return true;
                    }
                }
                fsmFish.food = null;
                return false;
            }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition ReachingToWander = new Transition("ReachingToWander",
            () => { return fsmFish.flocking; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition HomeToReach = new Transition("HomeToReach",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard_global.homeAttractor) < blackboard_global.homeCloseDistance; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition ReachToEat = new Transition("ReachToEat",
            () => { return SensingUtils.DistanceToTarget(gameObject,surrogateTarget) <= 1f; }, // write the condition checkeing code in {}
            () => { fsmFish.homeReached = true; }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition AnyToFlee = new Transition("AnyToFlee",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard_global.shark) < blackboard_global.fleeDistanceTrigger; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition FleeToPrevious = new Transition("FleeToPrevious",
            () => { return elpasedTime >= blackboard_global.fleeTime; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition PreviousToWander = new Transition("PreviousToWander",
            () => { return lastState == FlockingAround; }, // write the condition checkeing code in {}
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
        Transition PreviousToReachHome = new Transition("PreviousToHome",
        () => { return lastState == ReachingHome; }, // write the condition checkeing code in {}
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

        AddStates(FlockingAround, ReachingFood, GoingHome, Fleeing, PreviousState, ReachingHome);

        AddTransition(FlockingAround, WanderToReaching, ReachingFood);
        AddTransition(FlockingAround, AnyToFlee, Fleeing);
        AddTransition(ReachingFood, ReachingToHome, GoingHome);
        AddTransition(ReachingFood, ReachingToWander, FlockingAround);
        AddTransition(ReachingFood, AnyToFlee, Fleeing);
        AddTransition(GoingHome, AnyToFlee, Fleeing);
        AddTransition(GoingHome, HomeToReach, ReachingHome);
        AddTransition(ReachingHome,ReachToEat,FlockingAround);
        AddTransition(ReachingHome,AnyToFlee,Fleeing);
        AddTransition(Fleeing, FleeToPrevious, PreviousState);
        AddTransition(PreviousState, PreviousToWander, FlockingAround);
        AddTransition(PreviousState, PreviousToReach, ReachingFood);
        AddTransition(PreviousState, PreviousToHome, GoingHome);
        AddTransition(PreviousState, PreviousToReachHome, ReachingHome);

        initialState = FlockingAround;
    }
}