using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Shark_Alert", menuName = "Finite State Machines/FSM_Shark_Alert", order = 1)]
public class FSM_Shark_Alert : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    SteeringContext context;
    Arrive arrive;
    Seek seek;
    SHARK_BLAKCBOARD blackboard;
    GameObject theFish;
    GameObject soundTarget;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        context = GetComponent<SteeringContext>();
        arrive = GetComponent<Arrive>();
        seek = GetComponent<Seek>();
        blackboard = GetComponent<SHARK_BLAKCBOARD>();
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

        State GoHuntFish = new State("GoHuntFish",
            () => {
                context.maxAcceleration *= 1.5f;
                context.maxSpeed *= 1.5f;
                seek.target = theFish;
                seek.enabled = true;
            }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => {
                seek.enabled = false;
                context.maxAcceleration /= 1.5f;
                context.maxSpeed /= 1.5f;
            }  // write on exit logic inisde {}  
        );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------*/

        Transition FishDetected = new Transition("FishDetected",
            () => {
                theFish = SensingUtils.FindInstanceWithinRadius(gameObject, "BOID", blackboard.fishDetectableRadius);
                if (theFish != null)
                {
                    return SensingUtils.DistanceToTarget(gameObject, theFish) < blackboard.fishDetectableRadius;
                }
                return false;
            }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition FishReached = new Transition("FishReached",
            () => { return SensingUtils.DistanceToTarget(gameObject, theFish) < blackboard.fishReachedRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

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


        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

    }
}
