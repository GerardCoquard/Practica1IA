using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Ghost", menuName = "Finite State Machines/FSM_Ghost", order = 1)]
public class FSM_Ghost : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    Arrive arrive;
    Pursue pursue;
    GHOST_Blackboard blackboard;
    SteeringContext context;
    float elapsedTime;
    GameObject target;

    public override void OnEnter()
    {
        arrive = GetComponent<Arrive>();
        pursue = GetComponent<Pursue>();
        context = GetComponent<SteeringContext>();
        blackboard = GetComponent<GHOST_Blackboard>();
        arrive.target = blackboard.castle;
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        base.DisableAllSteerings();
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
        State goCastle = new State("GoCastle",
        () => { arrive.enabled = true; context.maxSpeed*=4; },
        () => { },
        () => { arrive.enabled = false; context.maxSpeed/=4; }
        );

        State hide = new State("Hide",
        () => { elapsedTime = 0; },
        () => { elapsedTime+=Time.deltaTime; },
        () => { }
        );

        State selectTarget = new State("SelectTarget",
        () => { },
        () => { },
        () => { }
        );

        State approach = new State("Approach",
        () => { pursue.target = target; pursue.enabled = true; },
        () => { },
        () => { }
        );

        State cryBoo = new State("CryBoo",
        () => { elapsedTime = 0; blackboard.CryBoo(true); },
        () => { elapsedTime+=Time.deltaTime; },
        () => { blackboard.CryBoo(false); pursue.enabled = false; }
        );

        Transition castleReached = new Transition("CastleReached",
            () => { return SensingUtils.DistanceToTarget(gameObject,blackboard.castle) <= blackboard.castleReachedRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition timeOut1 = new Transition("TimeOut1",
            () => { return elapsedTime >= blackboard.hideTime; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition targetSelected = new Transition("TragetSelected",
            () => { target = SensingUtils.FindRandomInstanceWithinRadius(gameObject,blackboard.victimLabel,blackboard.nerdDetectionRadius);
                    return target != null; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition targetIsClose = new Transition("TargetIsClose",
            () => { return SensingUtils.DistanceToTarget(gameObject,target) <= blackboard.closeEnoughToScare; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition timeOut2 = new Transition("TimeOut2",
            () => { return elapsedTime >= blackboard.booDuration; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */
        AddStates(goCastle,hide,selectTarget,approach,cryBoo);

        AddTransition(goCastle,castleReached,hide);
        AddTransition(hide,timeOut1,selectTarget);
        AddTransition(selectTarget,targetSelected,approach);
        AddTransition(approach,targetIsClose,cryBoo);
        AddTransition(cryBoo,timeOut2,goCastle);

        initialState = goCastle;

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
