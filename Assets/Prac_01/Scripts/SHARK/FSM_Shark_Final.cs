using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Shark_Final", menuName = "Finite State Machines/FSM_Shark_Final", order = 1)]
public class FSM_Shark_Final : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    SHARK_BLAKCBOARD blackboard;
    Flee flee;
    GameObject boat;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<SHARK_BLAKCBOARD>();
        flee = GetComponent<Flee>();
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
         *-----------------------------------------------*/

        State FleeFromBoat = new State("Flee From Boat",
            () => {
                flee.enabled = true;
                flee.target = boat;
            }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => {
                flee.enabled = false;
                flee.target = null;
            }  // write on exit logic inisde {}  
        );

        FiniteStateMachine SharkBehaviour = ScriptableObject.CreateInstance<FSM_Shark>();
        SharkBehaviour.name = "SharkBehaviour";


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------*/

        Transition BoatDetected = new Transition("Boat Detected",
            () => {
                boat = SensingUtils.FindInstanceWithinRadius(gameObject, "ATTACKER", blackboard.boatDetectedRadius);
                if (boat != null)
                    return true;
                return false;
            }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition BoatNoLongerDetected = new Transition("Boat No Longer Detected",
            () => {
            if (SensingUtils.DistanceToTarget(gameObject, boat) > blackboard.boatDetectedRadius)
                    return true;
                return false;
            }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------*/

        AddStates(SharkBehaviour, FleeFromBoat);

        AddTransition(SharkBehaviour, BoatDetected, FleeFromBoat);
        AddTransition(FleeFromBoat, BoatNoLongerDetected, SharkBehaviour);


        /* STAGE 4: set the initial state*/

        initialState = SharkBehaviour;

    }
}
