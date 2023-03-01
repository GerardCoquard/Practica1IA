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
    public Blackboard_Fish_Global blackboard_global;
    float elpasedTime;
    FSM_Fish fsmFish;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        flockingAround = GetComponent<FlockingAround>();
        blackboard_global = FindObjectOfType<Blackboard_Fish_Global>();
        fsmFish = (FSM_Fish)GetComponent<FSMExecutor>().fsm;
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
        () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.homeAttractor; elpasedTime = 0; fsmFish.wandering = true; blackboard_global.SetAllWandering(); fsmFish.isWaiting = false; }, // write on enter logic inside {}
        () => { elpasedTime += Time.deltaTime; }, // write in state logic inside {}
        () => { flockingAround.enabled = false; fsmFish.wandering = false; fsmFish.stateBefore = 1; }  // write on exit logic inisde {}
        );

        State ReachingFood = new State("Reaching Food",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.foodAttractor; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { flockingAround.enabled = false; fsmFish.stateBefore = 2; }  // write on exit logic inisde {}
        );

        State GoingHome = new State("Going Home",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.homeAttractor;}, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { flockingAround.enabled = false; fsmFish.stateBefore = 3; }  // write on exit logic inisde {}
        );
        State ReachingHome = new State("Reaching Home",
            () => { flockingAround.enabled = true; flockingAround.attractor = blackboard_global.homeAttractor; elpasedTime = 0; fsmFish.isReachingHome = true; }, // write on enter logic inside {}
            () => { elpasedTime += Time.deltaTime; }, // write in state logic inside {}
            () => { flockingAround.enabled = false; fsmFish.stateBefore = 4; }  // write on exit logic inisde {}
        );

        State EnterController = new State("EnterController",
        () => { Debug.Log(fsmFish.stateBefore); }, // write on enter logic inside {}
        () => {  }, // write in state logic inside {}
        () => { }  // write on exit logic inisde {}
        );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
        //Debug.Log(((FSM_Fish)GetComponent<FSMExecutor>().fsm).hungry);
        Transition WanderToReaching = new Transition("WanderToReaching",
           () => {  return fsmFish.hungry;  }, // write the condition checkeing code in {}
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        Transition ReachingToWander = new Transition("ReachingToWander",
            () => { return fsmFish.wandering; }, // write the condition checkeing code in {}
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

        Transition HomeToReach = new Transition("HomeToReach",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard_global.homeAttractor) < blackboard_global.homeCloseDistance; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition EnterToReachF = new Transition("EnterToReachF",
        () => { return fsmFish.stateBefore == 2; }, // write the condition checkeing code in {}
        () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition EnterToWander = new Transition("EnterToWander",
        () => { return fsmFish.stateBefore == 1; }, // write the condition checkeing code in {}
        () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition EnterToGoing = new Transition("EnterToGoing",
        () => { return fsmFish.stateBefore == 3; }, // write the condition checkeing code in {}
        () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition EnterToReachH = new Transition("EnterToReachH",
        () => { return fsmFish.stateBefore == 4; }, // write the condition checkeing code in {}
        () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(WanderingAround,ReachingFood,ReachingHome, GoingHome,EnterController);

        AddTransition(WanderingAround, WanderToReaching, ReachingFood);
        AddTransition(ReachingFood, ReachingToHome, GoingHome);
        AddTransition(ReachingFood, ReachingToWander, WanderingAround);
        AddTransition(GoingHome, HomeToReach, ReachingHome);

        AddTransition(EnterController, EnterToWander, WanderingAround);
        AddTransition(EnterController, EnterToReachF, ReachingFood);
        AddTransition(EnterController, EnterToGoing, GoingHome);
        AddTransition(EnterController, EnterToReachH, ReachingHome);


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = EnterController;


    }
}
