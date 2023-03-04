using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_FishFather", menuName = "Finite State Machines/FSM_FishFather", order = 1)]
public class FSM_FishFather : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    public Blackboard_Fish_Global blackboard_global;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard_global = FindObjectOfType<Blackboard_Fish_Global>();
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
        FiniteStateMachine EAT = ScriptableObject.CreateInstance<FSM_FishEat>();
        EAT.name = "EAT";
        FiniteStateMachine FISH = ScriptableObject.CreateInstance<FSM_Fish>();
        FISH.name = "FISH";


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
        Transition HomeToEat = new Transition("HomeToReach",
        () => { if(SensingUtils.DistanceToTarget(gameObject, blackboard_global.homeAttractor) < blackboard_global.homeCloseDistance && blackboard_global.reaching)
            {
                blackboard_global.reaching = false;
                return true;
            }
            return false;
             }, // write the condition checkeing code in {}
        () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        Transition WaitToWander = new Transition("WaitToWander",
        () => {
            if (blackboard_global.AllEated() && blackboard_global.waiting)
            {
                blackboard_global.waiting = false;
                return true;
            }
            return false;
        }, // write the condition checkeing code in {}
        () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(EAT,FISH);
        AddTransition(FISH, HomeToEat, EAT);
        AddTransition(EAT, WaitToWander, FISH);



        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = FISH;

    }
}
