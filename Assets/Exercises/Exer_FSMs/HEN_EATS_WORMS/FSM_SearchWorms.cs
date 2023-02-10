using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_SearchWorms", menuName = "Finite State Machines/FSM_SearchWorms", order = 1)]
public class FSM_SearchWorms : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private HEN_Blackboard blackboard;
    private WanderAround wanderAround;
    private Arrive arrive;
    private AudioSource audioSource;
    private GameObject theWorm;
    private float elapsedTime;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        /* COMPLETE */
        blackboard = GetComponent<HEN_Blackboard>();
        wanderAround = GetComponent<WanderAround>();
        arrive = GetComponent<Arrive>();
        audioSource = GetComponent<AudioSource>();
        wanderAround.attractor = blackboard.attractor;

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
       
        /* COMPLETE */
        base.DisableAllSteerings();
        audioSource.Stop();

        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* COMPLETE */
        
        State wander = new State("Wander",
        () => { wanderAround.enabled = true; audioSource.clip = blackboard.cluckingSound; audioSource.Play(); },
        () => { },
        () => { wanderAround.enabled = false;audioSource.Stop(); }
        );

        State reachWorm = new State("ReachWorm",
        () => { arrive.target = theWorm; arrive.enabled = true; },
        () => { },
        () => { arrive.enabled = false; }
        );

        State eat = new State("Eat",
        () => {elapsedTime = 0; audioSource.clip = blackboard.eatingSound; audioSource.Play();},
        () => { elapsedTime+=Time.deltaTime;},
        () => { Destroy(theWorm);}
        );
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */
        Transition wormDetected = new Transition("WormDetected",
            () => { theWorm = SensingUtils.FindInstanceWithinRadius(gameObject,"WORM",blackboard.wormDetectableRadius);
            return theWorm != null;}, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition wormReached = new Transition("WormReached",
            () => { return SensingUtils.DistanceToTarget(gameObject,theWorm) < blackboard.wormReachedRadius;},
            // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition wormVanished = new Transition("WormVanished",
            () => { return theWorm == null || theWorm.Equals(null);}, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition timeOut = new Transition("TimeOut",
            () => { return elapsedTime >= blackboard.timeToEatWorm;}, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        AddStates(wander,reachWorm,eat);

        AddTransition(wander,wormDetected,reachWorm);
        AddTransition(reachWorm,wormVanished,wander);
        AddTransition(reachWorm,wormReached,eat);
        AddTransition(eat,timeOut,wander);
        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        initialState = wander;
        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
    }
}
