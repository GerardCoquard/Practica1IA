using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_Shar_Hunt", menuName = "Finite State Machines/FSM_Shar_Hunt", order = 1)]
public class FSM_Shar_Hunt : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    SteeringContext context;
    WanderAround wander;
    Arrive arrive;
    Seek seek;
    SHARK_BLAKCBOARD blackboard;
    GameObject theFish;
    float timeEating;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        context = GetComponent<SteeringContext>();
        wander = GetComponent<WanderAround>();
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

        State WanderAroundHome = new State("WanderAroundHome",
            () => {
                Debug.Log("SADAS");
                wander.attractor = blackboard.attractor; 
                wander.enabled = true; 
            }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { wander.enabled = false; }  // write on exit logic inisde {}  
        );

        State GoHuntFish = new State("GoHuntFish",
            () => {
                context.maxAcceleration *= 2;
                context.maxSpeed *= 2f;
                seek.target = theFish;
                seek.enabled = true;
            }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { 
                seek.enabled = false;
                context.maxAcceleration /= 2;
                context.maxSpeed /= 2f;
            }  // write on exit logic inisde {}  
        );

        State GoingHomeToEatFish = new State("GoingHomeToEatFish",
            () => {
                theFish.GetComponent<FSMExecutor>().enabled = false;
                theFish.GetComponent<FlockingAround>().enabled = false;
                theFish.transform.parent = gameObject.transform;
                arrive.target = blackboard.home;
                arrive.enabled = true;
            }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { arrive.enabled = false; }  // write on exit logic inisde {}  
        );

        State EatingFish = new State("EatingFish",
            () => { }, // write on enter logic inside {}
            () => { timeEating += Time.deltaTime; }, // write in state logic inside {}
            () => { 
                timeEating = 0;
                gameObject.transform.localScale *= 1.1f;
                theFish.SetActive(false);
            }  // write on exit logic inisde {}  
        );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------*/

        Transition FishDetected = new Transition("FishDetected",
            () => {
                theFish = SensingUtils.FindInstanceWithinRadius(gameObject, "BOID", blackboard.fishDetectableRadius);
                return SensingUtils.DistanceToTarget(gameObject, theFish) < blackboard.fishDetectableRadius;
            }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition FishReached = new Transition("FishReached",
            () => { return SensingUtils.DistanceToTarget(gameObject, theFish) < blackboard.fishReachedRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition FishEated = new Transition("FishEated",
            () => { return timeEating > blackboard.timeToEat; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition HomeReached = new Transition("HomeReached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.home) < blackboard.homeReachedRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------*/

        AddStates(WanderAroundHome, GoHuntFish, GoingHomeToEatFish, EatingFish);

        AddTransition(WanderAroundHome, FishDetected, GoHuntFish);
        AddTransition(GoHuntFish, FishReached, GoingHomeToEatFish);
        AddTransition(GoingHomeToEatFish, HomeReached, EatingFish);
        AddTransition(EatingFish, FishEated, WanderAroundHome);


        /* STAGE 4: set the initial state*/

        initialState = WanderAroundHome;

    }
}
