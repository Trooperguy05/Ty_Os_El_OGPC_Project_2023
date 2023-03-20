using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public interface IState
{
    public string name { get; }
    public void Enter();
    public void Execute();
    public void Exit();
}

public class MonsterStateMachine
{
    public IState currentState;
 
    public void ChangeState(IState newState)
    {
        if (currentState != null) currentState.Exit();
 
        currentState = newState;
        currentState.Enter();
    }
 
    public void Update()
    {
        if (currentState != null) currentState.Execute();
    }
}

// monster is idle or standing
public class StandState : IState
{
    MonsterMovementNavmesh owner;

    public StandState(MonsterMovementNavmesh owner) { this.owner = owner; }

    public string name { get { return "StandState"; } }

    public void Enter() {
        Debug.Log(owner.gameObject.name + " entering Stand State");

        // reset
        owner.stopEverything();

        // reset moving
        owner.moving = false;
    }

    public void Execute() { }

    public void Exit() { Debug.Log(owner.gameObject.name + " exiting Stand State"); }
}

// enemy chases the player
public class ChaseState : IState
{
    MonsterMovementNavmesh owner;

    public ChaseState(MonsterMovementNavmesh owner) { this.owner = owner; }

    public string name { get { return "ChaseState"; } }

    public void Enter() { 
        Debug.Log(owner.gameObject.name + " entering Chase State");

        // reset
        owner.stopEverything();

        //owner.moveToTarget_wrapper();
    }
    
    public void Execute() { }

    public void Exit() { Debug.Log(owner.gameObject.name + " exiting Chase State"); }
}

// wander state: chooses a random node and travels to it
public class WanderState : IState
{
    MonsterMovementNavmesh owner;

    public WanderState(MonsterMovementNavmesh owner) { this.owner = owner; }

    public string name { get { return "WanderState"; } }

    public void Enter() { 
        Debug.Log(owner.gameObject.name + " entering Wander State");

        // reset
        owner.stopEverything();

        //owner.monsterWander_wrapper();
    }
    
    public void Execute() { }

    public void Exit() { Debug.Log(owner.gameObject.name + " exiting Wander State"); }
}

// investigate state: go to point closest to sound created by player
public class InvestigateState : IState
{
    MonsterMovementNavmesh owner;

    public InvestigateState(MonsterMovementNavmesh owner) { this.owner = owner; }

    public string name { get { return "InvestigateState"; } }

    public void Enter() { 
        Debug.Log(owner.gameObject.name + " entering Investigate State");

        // reset
        owner.stopEverything();

        owner.monsterInvestigate_wrapper();
    }
    
    public void Execute() { }

    public void Exit() { Debug.Log(owner.gameObject.name + " exiting Investigate State"); }
}

// enemy transitions between moving to calculating next node
public class TransitionState : IState
{
    MonsterMovementNavmesh owner;

    public TransitionState(MonsterMovementNavmesh owner) { this.owner = owner; }

    public string name { get { return "TransitionState"; } }

    public void Enter() { 
        Debug.Log(owner.gameObject.name + " entering Transition State");

        // reset
        owner.stopEverything();
    }
    
    public void Execute() { }

    public void Exit() { Debug.Log(owner.gameObject.name + " exiting Transition State"); }
}

/// main script
public class MonsterMovementNavmesh : MonoBehaviour
{
    /// directionVector = destination - origin

    [HideInInspector] public MonsterStateMachine stateMachine = new MonsterStateMachine();

    [Header("Action Bools")]
    public bool isWandering = false;
    public bool isChasing = false;
    public bool isInvestigating = false;

    [Header("Movement Variables")]
    public bool moving = false;
    public bool reachedTarget = false;
    private Rigidbody rb;
    private NavMeshAgent nA;

    [Header("Sound Detection")]
    public GameObject soundLocationPrefab;
    private MonsterSoundDetection mSD;
    private PlayerSoundRadius pSR;

    [Header("Suspicion")]
    private MonsterSuspicion mS;

    // get stuff
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mSD = transform.GetChild(0).GetComponent<MonsterSoundDetection>();
        pSR = GameObject.Find("Player").GetComponent<PlayerSoundRadius>();
        nA = GetComponent<NavMeshAgent>();
        mS = GetComponent<MonsterSuspicion>();

        // set default state
        stateMachine.ChangeState(new StandState(this));
    }

    // Update is called once per frame
    void Update()
    {   
        /// sound detection \\\
        if (mSD.inEarshot && pSR.soundValue >= mSD.soundSensitivity) {
            if (stateMachine.currentState.name != "InvestigateState") {
                isInvestigating = true;
                stateMachine.ChangeState(new InvestigateState(this));
            }
        }

        /// state handler \\\
        // wander state
        if (isWandering && stateMachine.currentState.name != "WanderState") {
            stateMachine.ChangeState(new WanderState(this));
        }

        // investigate state
        if (isInvestigating && stateMachine.currentState.name != "InvestigateState") {
            stateMachine.ChangeState(new InvestigateState(this));
        }

        // stand state
        if (!(isWandering || isInvestigating || isChasing) && stateMachine.currentState.name != "StandState") {
            stateMachine.ChangeState(new StandState(this));
        }
    }

    // method that checks if the monster arrived at its destination
    public bool monsterArrived() {
        if (!nA.pathPending) {
            if (nA.remainingDistance <= nA.stoppingDistance) {
                if (nA.hasPath || nA.velocity.sqrMagnitude == 0f) {
                    return true;
                }
            }
        }
        return false;
    }

    // method that allows the monster to wander around the map \\
    private IEnumerator monsterInvestigate() {
        mS.suspicion += 50;
        do {
            nA.destination = mSD.pointOfSound;
            yield return null;
        } while (!monsterArrived());
        isInvestigating = false;
        yield return new WaitForSeconds(1f);
    }
    public void monsterInvestigate_wrapper() { StartCoroutine(monsterInvestigate()); }

    // method that stops all invokes and coroutines on the monobehaviour
    public void stopEverything() {
        CancelInvoke();
        StopAllCoroutines();
    }
}
