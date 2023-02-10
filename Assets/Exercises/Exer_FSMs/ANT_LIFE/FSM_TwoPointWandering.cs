using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_TwoPointWandering", menuName = "Finite State Machines/FSM_TwoPointWandering", order = 1)]
public class FSM_TwoPointWandering : FiniteStateMachine
{
    

    private WanderAround wanderAround;
    private SteeringContext steeringContext;
    private ANT_Blackboard blackboard;

    private float elapsedTime = 0;


    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is executed every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<ANT_Blackboard>();
        wanderAround = GetComponent<WanderAround>();
        steeringContext = GetComponent<SteeringContext>();

        /* COMPLETE */

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */

        /* COMPLETE */

        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         */

        State goingA = new State("Going_A",
           () => {elapsedTime = 0; wanderAround.enabled = true; wanderAround.attractor = blackboard.locationA;},
           () => {elapsedTime += Time.deltaTime;}, 
           () => {wanderAround.enabled = false;}
       );

        State goingB = new State("Going_B",
           () => { elapsedTime = 0; wanderAround.enabled = true; wanderAround.attractor = blackboard.locationB;},
           () => { elapsedTime += Time.deltaTime;},
           () => {wanderAround.enabled = false;}
       );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------
        */

        
        Transition LocationAreached = new Transition("LocationAReached",
            () => {return SensingUtils.DistanceToTarget(gameObject,blackboard.locationA) < blackboard.loationReachedRadius;}, // write the condition checkeing code in {}
            () => {steeringContext.seekWeight = blackboard.initalSeekWeight;}  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition LocationBreached = new Transition("LocationBReached",
            () => {return SensingUtils.DistanceToTarget(gameObject,blackboard.locationB) < blackboard.loationReachedRadius;}, // write the condition checkeing code in {}
            () => {steeringContext.seekWeight = blackboard.initalSeekWeight;}  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
        Transition TimeOut = new Transition("TimeOut",
            () => {return elapsedTime >= blackboard.intervalBetweentimeOuts;}, // write the condition checkeing code in {}
            () => {steeringContext.seekWeight = Mathf.Clamp(steeringContext.seekWeight += blackboard.seekIncrement,0,1); elapsedTime  =0;}  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        /* COMPLETE, create the transitions */

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
         */

        AddStates(goingA, goingB);

        /* COMPLETE, add the transitions */
        AddTransition(goingA,LocationAreached,goingB);
        AddTransition(goingB,LocationBreached,goingA);
        AddTransition(goingA,TimeOut,goingA);
        AddTransition(goingB,TimeOut,goingB);

        /* STAGE 4: set the initial state */

        initialState = goingA;
    }
}
