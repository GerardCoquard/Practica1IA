using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Shark", menuName = "Finite State Machines/FSM_Shark", order = 1)]
public class FSM_Shark : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private SHARK_BLAKCBOARD blackboard;
    private Arrive arrive;
    private WanderAround wander;
    private float time;
    GameObject soundTarget;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<SHARK_BLAKCBOARD>();
        arrive = GetComponent<Arrive>();
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        //DisableAllSteerings();
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
        FiniteStateMachine HUNT = ScriptableObject.CreateInstance<FSM_Shar_Hunt>();
        HUNT.name = "HUNT";

        State CheckingSound = new State("Checking Sound",
            () => {
                arrive.enabled = true;
                arrive.target = soundTarget;
            }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => {
                arrive.enabled = false;
                arrive.target = null;
            }  // write on exit logic inisde {}  
        );

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
        Transition SoundHeard = new Transition("SoundHeard",
        () => {


        soundTarget = SensingUtils.FindInstanceWithinRadius(gameObject, "RED_TAG", blackboard.soundDetectableRadius);
        if (soundTarget != null)
            return true;
        return false;
        }, // write the condition checkeing code in {}
        () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition SoundDisappear = new Transition("SoundDisappear",
            () => {
                if (soundTarget.Equals(null))
                    return true;
                return false;
            }, // write the condition checkeing code in {}
            () => { }
        );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        AddStates(HUNT, CheckingSound);
        AddTransition(HUNT, SoundHeard, CheckingSound);
        AddTransition(CheckingSound, SoundDisappear, HUNT);





        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = HUNT;

    }
}
