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
        FiniteStateMachine FLEE = ScriptableObject.CreateInstance<FSM_FishFlee>();
        FLEE.name = "FLEE";

        FiniteStateMachine HUNGRY = ScriptableObject.CreateInstance<FSM_FishHungry>();
        HUNGRY.name = "HUNGRY";


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
       
        //Transition WanderToFlee = new Transition("WanderToFlee",
        //    () => { return SensingUtils.DistanceToTarget(gameObject,blackboard_global.shark) < blackboard_global.fleeDistanceTrigger; }, // write the condition checkeing code in {}
        //    () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        //);
        
        
        //Transition ReachingToFlee = new Transition("ReachingToFlee",
        //    () => { return SensingUtils.DistanceToTarget(gameObject,blackboard_global.shark) < blackboard_global.fleeDistanceTrigger; }, // write the condition checkeing code in {}
        //    () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        //);       
        //Transition HomeToFlee = new Transition("HomeToFlee",
        //    () => { return SensingUtils.DistanceToTarget(gameObject,blackboard_global.shark) < blackboard_global.fleeDistanceTrigger; }, // write the condition checkeing code in {}
        //    () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        //);
       
        //Transition ReachToFlee = new Transition("ReachToFlee",
        //    () => { return SensingUtils.DistanceToTarget(gameObject,blackboard_global.shark) < blackboard_global.fleeDistanceTrigger; }, // write the condition checkeing code in {}
        //    () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        //);
        
        //Transition FleeToPrevious = new Transition("FleeToPrevious",
        //    () => { return elpasedTime >= blackboard_global.fleeTime; }, // write the condition checkeing code in {}
        //    () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        //);


        //Transition PreviousToWander = new Transition("PreviousToWander",
        //    () => { return lastState == WanderingAround; }, // write the condition checkeing code in {}
        //    () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        //);
        //Transition PreviousToReach = new Transition("PreviousToReach",
        //    () => { return lastState == ReachingFood; }, // write the condition checkeing code in {}
        //    () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        //);
        //Transition PreviousToHome = new Transition("PreviousToHome",
        //    () => { return lastState == GoingHome; }, // write the condition checkeing code in {}
        //    () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        //);
        



        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);
        

        AddTransition(sourceState, transition, destinationState);

         */ 


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

        AddStates(HUNGRY,FLEE);

       
        //AddTransition(WanderingAround,WanderToFlee,Fleeing);

        //AddTransition(ReachingFood,ReachingToFlee,Fleeing);
        
        //AddTransition(GoingHome,HomeToFlee,Fleeing);

        //AddTransition(Fleeing,FleeToPrevious,PreviousState);
        //AddTransition(PreviousState,PreviousToWander,WanderingAround);
        //AddTransition(PreviousState,PreviousToReach,ReachingFood);
        //AddTransition(PreviousState,PreviousToHome,GoingHome);

        initialState = HUNGRY;
    }
}
