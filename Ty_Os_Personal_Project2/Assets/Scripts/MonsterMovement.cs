using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    MonsterMovement owner;

    public StandState(MonsterMovement owner) { this.owner = owner; }

    public string name { get { return "StandState"; } }

    public void Enter() {
        Debug.Log(owner.gameObject.name + " entering Stand State");

        // reset
        owner.stopEverything();
    }

    public void Execute() { }

    public void Exit() { Debug.Log(owner.gameObject.name + " exiting Stand State"); }
}

/// main script
public class MonsterMovement : MonoBehaviour
{
    /// directionVector = destination - origin

    MonsterStateMachine stateMachine = new MonsterStateMachine();

    [Header("Movement Node Variables")]
    public bool reachedDestination = false;
    public GameObject currentNode;
    public GameObject movingToNode;
    private Rigidbody rb;

    // pathfinding \\
    /*
    [Header("Pathfinding Variables")]
    public float speed = 200f;
    public Transform target;
    public float nextWaypointDistance = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody rb;
    */

    // get stuff
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        stateMachine.ChangeState(new StandState(this));
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    // physics
    void FixedUpdate() {
        // only chase player if the blob is in the chase state
        if (stateMachine.currentState.name == "StandState") {
            if (path == null) return;
            if (currentWaypoint >= path.vectorPath.Count) {
                reachedEndOfPath = true;
                return;
            }
            else {
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - new Vector2(rb.position.x, rb.position.z)).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force*2);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance) currentWaypoint++; 
        }
    }
    */

    // method that stops all invokes and coroutines on the monobehaviour
    public void stopEverything() {
        CancelInvoke();
        StopAllCoroutines();
    }
}
