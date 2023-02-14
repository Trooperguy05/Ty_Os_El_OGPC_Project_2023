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

// enemy chases the player
public class ChaseState : IState
{
    MonsterMovement owner;

    public ChaseState(MonsterMovement owner) { this.owner = owner; }

    public string name { get { return "ChaseState"; } }

    public void Enter() { 
        Debug.Log(owner.gameObject.name + " entering Chase State");

        // reset
        owner.stopEverything();

        owner.moveToTarget_wrapper();
    }
    
    public void Execute() { }

    public void Exit() { Debug.Log(owner.gameObject.name + " exiting Chase State"); }
}

// wander state: chooses a random node and travels to it
public class WanderState : IState
{
    MonsterMovement owner;

    public WanderState(MonsterMovement owner) { this.owner = owner; }

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

// enemy transitions between moving to calculating next node
public class TransitionState : IState
{
    MonsterMovement owner;

    public TransitionState(MonsterMovement owner) { this.owner = owner; }

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
public class MonsterMovement : MonoBehaviour
{
    /// directionVector = destination - origin

    MonsterStateMachine stateMachine = new MonsterStateMachine();

    [Header("Movement Node Variables")]
    public bool moving = false;
    public bool reachedTarget = false;
    public float speed;
    public GameObject previousNode;
    public GameObject currentNode;
    public GameObject nextNode;
    public Transform target;
    private Vector3 moveDir;
    private Rigidbody rb;

    [Header("Adjacent Nodes")]
    public List<GameObject> nodeAdjacents = new List<GameObject>();

    // get stuff
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        stateMachine.ChangeState(new StandState(this));

        speed = Mathf.Pow(2, (-speed+1));
    }

    // Update is called once per frame
    void Update()
    {   
        /*
        if (Input.GetKeyDown(KeyCode.H) && stateMachine.currentState.name != "ChaseState") {
            stateMachine.ChangeState(new ChaseState(this));
        }
        */
        if (Input.GetKeyDown(KeyCode.G) && stateMachine.currentState.name != "StandState") {
            stateMachine.ChangeState(new StandState(this));
        }
        // state handler
        /*
        if (stateMachine.currentState.name != "ChaseState") {
            stateMachine.ChangeState(new ChaseState(this));
            StartCoroutine(moveToTarget());
        }
        */

        // wander state
        if (Input.GetKeyDown(KeyCode.H) && stateMachine.currentState.name != "WanderState") {
            stateMachine.ChangeState(new WanderState(this));
        }
    }

    // method that gets the adjacent nodes from current nod
    private void getAdjacentNodes() {
        // reset list
        for (int i = 0; i < nodeAdjacents.Count; i++) {
            nodeAdjacents.RemoveAt(0);
        }

        // get new list
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
        foreach(Collider hit in hitColliders) {
            if (hit.gameObject.tag == "movement node") {
                nodeAdjacents.Add(hit.gameObject);
            }
        }
    }

    // method that checks whether an adjacent node node is closer to its target, and set as next node
    private void movingToNode() {
        // set up
        int nodeIndex = -1;
        float closestDistance = Vector3.Distance(transform.position, target.position);

        // check each node in node adjacents
        for (int i = 0; i < nodeAdjacents.Count; i++) {
            float distance = Vector3.Distance(nodeAdjacents[i].transform.position, target.position);
            if (distance < closestDistance && nodeAdjacents[i] != previousNode) {
                nodeIndex = i;
                closestDistance = distance;
            }
        }
        if (nodeIndex != -1) {
            nextNode = nodeAdjacents[nodeIndex];
        }
        else {
            stopEverything();
            stateMachine.ChangeState(new StandState(this));
            reachedTarget = true;
        }
    }

    // method that lerps the monster to move it
    private IEnumerator moveMonster() {
        // if there is no next node
        if (reachedTarget) yield break;

        // set up
        moving = true;
        float currentTime = 0f;

        // move monster
        while (currentTime < speed) {
            currentTime += Time.deltaTime;
            Vector3 movedPosition = Vector3.Lerp(transform.position, nextNode.transform.position, currentTime / speed);
            transform.position = new Vector3(movedPosition.x, transform.position.y, movedPosition.z);
            yield return null;
        }
        
        // update nodes
        previousNode = currentNode;
        currentNode = nextNode;
        nextNode = null;

        // get new adjacent nodes
        getAdjacentNodes();

        // set moving to false
        moving = false;
    }

    // method that handles the monster wandering around the map
    public IEnumerator monsterWander() {
        bool wandering = false;
        while (true) {
            if (!wandering) {
                wandering = true;
                GameObject[] nodes = GameObject.FindGameObjectsWithTag("movement node");
                target = nodes[Random.Range(0, nodes.Length-1)].transform;
                moveToTarget_wrapper();
            }
            if (reachedTarget) {
                wandering = false;
            }
            yield return null;
        }
    }
    public void monsterWander_wrapper() { StartCoroutine(monsterWander()); }

    // method that combines methods to move the monster
    private IEnumerator moveToTarget() {
        reachedTarget = false;
        getAdjacentNodes();
        while (!reachedTarget) {
            movingToNode();
            StartCoroutine(moveMonster());
            while (moving) yield return null;
            yield return null;
        }
    }
    public void moveToTarget_wrapper() { StartCoroutine(moveToTarget()); }

    // method that stops all invokes and coroutines on the monobehaviour
    public void stopEverything() {
        CancelInvoke();
        StopAllCoroutines();
    }
}
