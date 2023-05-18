using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

// state machine template
public interface IState
{
    public string name { get; }
    public void Enter();
    public void Execute();
    public void Exit();
}

// monster state machine
public class MonsterStateMachine
{
    // the current state
    public IState currentState;
    
    // when state changes
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

        owner.stopMonster();
    }

    public void Execute() { }

    public void Exit() { Debug.Log(owner.gameObject.name + " exiting Stand State"); }
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

        owner.monsterWander_wrapper();
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

/// main script
public class MonsterMovementNavmesh : MonoBehaviour
{
    /// directionVector = destination - origin

    [HideInInspector] public MonsterStateMachine stateMachine = new MonsterStateMachine();

    [Header("Movement Variables")]
    public bool AIOn = true;
    public bool moving = false;
    public bool reachedTarget = false;
    public float waitTime;
    private Rigidbody rb;
    private NavMeshAgent nA;
    private bool isInvestigating = false;

    [Header("Sound Detection")]
    public GameObject soundLocationPrefab;
    private MonsterSoundDetection mSD;
    private PlayerSoundRadius pSR;

    [Header("Suspicion")]
    private MonsterSuspicion mS;

    [Header("Grid")]
    public Vector3 gridSize;
    public LayerMask groundLayer;

    [Header("Animations")]
    private Animator monsterAnim;

    // get stuff
    void Start()
    {
        // Get Stuff \\
        rb = GetComponent<Rigidbody>();
        mSD = transform.GetChild(2).GetComponent<MonsterSoundDetection>();
        pSR = GameObject.Find("Player").GetComponent<PlayerSoundRadius>();
        nA = GetComponent<NavMeshAgent>();
        mS = GetComponent<MonsterSuspicion>();
        monsterAnim = GetComponent<Animator>();

        // set default state
        stateMachine.ChangeState(new StandState(this));
    }

    // Update is called once per frame
    void Update()
    {   
        if (AIOn) {
            /// State Handler \\\
            // sound detection
            if (mSD.inEarshot && pSR.soundValue >= mSD.soundSensitivity) {
                if (stateMachine.currentState.name != "InvestigateState" && !isInvestigating) {
                    stateMachine.ChangeState(new InvestigateState(this));
                }
            }
            else {
                if (stateMachine.currentState.name != "WanderState" && !isInvestigating) {
                    stateMachine.ChangeState(new WanderState(this));
                }
            }
        }
        else {
            if (stateMachine.currentState.name != "StandState") stateMachine.ChangeState(new StandState(this));
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

    // method that allows the monster to wander across the map \\
    private IEnumerator monsterWander() {
        do {
            mS.sawFlashlight = false;
            // find random position on map \\
            // random location
            float ranX = Random.Range(-gridSize.x, 0);
            float ranZ = Random.Range(-gridSize.z, gridSize.z);

            // set up
            Vector3 ranPos = new Vector3(ranX, 2f, ranZ);
            bool isGround = false;
            
            // check if ground is underneath
            isGround = Physics.Raycast(ranPos, Vector3.down, 10f, groundLayer);

            // set the destination
            if (isGround) nA.destination = ranPos;

            // trigger walk animation
            monsterAnim.SetTrigger("StartWalking");
            
            // while the monster is going to position, stop looking for a random position
            while (!monsterArrived()) yield return null;
        } while (true);
    }
    public void monsterWander_wrapper() { StartCoroutine(monsterWander()); }

    // method that allows the monster to investigate sounds around the map \\
    private IEnumerator monsterInvestigate() {
        isInvestigating = true;
        // investigate
        StartCoroutine(mS.updateSuspicion(10));
        // trigger walk animation
        monsterAnim.SetTrigger("StartWalking");

        // walk towards point of sound
        nA.destination = mSD.pointOfSound;
        while (!monsterArrived()) yield return null;

        // 'look' around \\
        // trigger stand animation
        monsterAnim.SetTrigger("StartStanding");
        yield return new WaitForSeconds(waitTime);
        isInvestigating = false;
    }
    public void monsterInvestigate_wrapper() { StartCoroutine(monsterInvestigate()); }

    // method that stops the monster when it enters stand state
    public void stopMonster() {
        nA.destination = transform.position;
    }

    // method that stops all invokes and coroutines on the monobehaviour
    public void stopEverything() {
        CancelInvoke();
        StopAllCoroutines();
    }
}
