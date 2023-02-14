using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    MonsterMovement owner;

    public StandState(MonsterMovement owner) { this.owner = owner; }

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

// investigate state: go to point closest to sound created by player
public class InvestigateState : IState
{
    MonsterMovement owner;

    public InvestigateState(MonsterMovement owner) { this.owner = owner; }

    public string name { get { return "InvestigateState"; } }

    public void Enter() { 
        Debug.Log(owner.gameObject.name + " entering Investigate State");

        // reset
        owner.stopEverything();
    }
    
    public void Execute() { }

    public void Exit() { Debug.Log(owner.gameObject.name + " exiting Investigate State"); }
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

    [Header("Action Bools")]
    public bool isWandering = false;
    public bool isChasing = false;
    public bool isInvestigating = false;

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

    [Header("Sound Detection")]
    private MonsterSoundDetection mSD;

    // get stuff
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mSD = transform.GetChild(0).GetComponent<MonsterSoundDetection>();

        stateMachine.ChangeState(new StandState(this));

        speed = Mathf.Pow(2, (-speed+1));
    }

    // Update is called once per frame
    void Update()
    {   
        /// sound detection \\\
        // pain

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

        // remove dupes
        nodeAdjacents = nodeAdjacents.Distinct().ToList();
    }

    // method that checks whether an adjacent node node is closer to its target, and set as next node
    private void movingToNode() {
        // set up
        int nodeIndex = -1;
        float closestDistance = Vector3.Distance(transform.position, target.position);

        // check each node in node adjacents
        for (int i = 0; i < nodeAdjacents.Count; i++) {
            // closest node
            float distance = Vector3.Distance(nodeAdjacents[i].transform.position, target.position);
            if (distance < closestDistance && nodeAdjacents[i] != previousNode) {
                nodeIndex = i;
                closestDistance = distance;

                // if target node is closest, set the target to it
                if (nodeAdjacents[i].name == target.gameObject.name) { break; }
            }
        }
        // if there is a next node, continue to it
        if (nodeIndex != -1) {
            nextNode = nodeAdjacents[nodeIndex];
        }
        else {
            //stopEverything();
            //stateMachine.ChangeState(new StandState(this));
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

        // get adjacent nodes
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

        // if the monster was inbetween nodes, reset
        transform.position = currentNode.transform.position;
    }
}
