using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{   
    [Header("Tag List")]
    public List<string> tagList = new List<string>();

    [Header("Node List")]
    public List<GameObject> nodeList = new List<GameObject>();

    [Header("GameObjects")]
    public GameObject nodePrefab;
    public GameObject monster;

    [Header("Node Grid Calcs")]
    public Transform gridOrigin;
    public Vector2 gridSize;
    public float nodeSpacing;
    private Vector2 nodePlacement;

    void Awake() {
        createGrid();
    }
    
    // method that creates the node grid for the monster
    public void createGrid() {
        List<GameObject> nodesCreated = new List<GameObject>();

        // set up grid
        nodePlacement = new Vector2((gridOrigin.transform.position.x-gridSize.x), (gridOrigin.transform.position.z+gridSize.y));

        // instantiate nodes
        int count = 0;
        for (int i = 0; i < gridSize.x; i++) {
            for (int j = 0; j < gridSize.y; j++) {
                if (checkSpacing()) {
                    GameObject foo = Instantiate(nodePrefab, new Vector3(nodePlacement.x, 2f, nodePlacement.y), nodePrefab.transform.rotation);
                    nodesCreated.Add(foo);
                    nodeList.Add(foo);
                    foo.name = "Movement Node " + count;
                    count++;
                }
                nodePlacement.x += nodeSpacing * 1.25f;
            }
            nodePlacement.x = (gridOrigin.transform.position.x-gridSize.x);
            nodePlacement.y -= nodeSpacing * 1.25f;
        }

        // set the position of the monster
        GameObject randomNode = nodesCreated[Random.Range(0, nodesCreated.Count)];
        monster.transform.position = new Vector3(randomNode.transform.position.x, monster.transform.position.y, randomNode.transform.position.z);
        monster.GetComponent<MonsterMovement>().currentNode = randomNode;
    }

    // method that checks the spacing before placing a node
    public bool checkSpacing() {
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(nodePlacement.x, 2f, nodePlacement.y), nodeSpacing);

        // check space
        foreach(Collider hit in hitColliders) {
            // no space
            if (tagList.Contains(hit.gameObject.tag)) {
                return false;
            }
        }
        // is space
        return true;
    }

    /*
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;

        foreach(GameObject node in nodeList) {
            Gizmos.DrawWireSphere(node.transform.position, nodeSpacing);
        }
    }
    */
}
