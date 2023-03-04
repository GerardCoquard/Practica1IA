using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_FishEat", menuName = "Finite State Machines/FSM_FishEat", order = 1)]
public class FSM_FishEat : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    public Blackboard_Fish_Global blackboard_global;
    float elpasedTime;
    FSM_Fish fsmFish;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
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
        State Eating = new State("Eating",
        () => { elpasedTime = 0; fsmFish.eating = true; }, // write on enter logic inside {}
        () => { elpasedTime += Time.deltaTime;  }, // write in state logic inside {}
        () => { fsmFish.eating = false; }  // write on exit logic inisde {}
        );
        State WaitingForAllAte = new State("Waiting For All Ate",
            () => { blackboard_global.waiting = true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { fsmFish.hungry = false; }  // write on exit logic inisde {}
        );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
        Transition EatToWait = new Transition("EatToWait",
    () => { return elpasedTime >= blackboard_global.eatTime; }, // write the condition checkeing code in {}
    () => {
        if (fsmFish.food != null)
        {
            fsmFish.food.SetActive(false);
            fsmFish.food = null;
        }
    }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
);


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddStates(Eating, WaitingForAllAte);

        AddTransition(Eating, EatToWait, WaitingForAllAte);

        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = Eating;

    }
}
