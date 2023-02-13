using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    [Header("Node List")]
    public List<GameObject> nodeList = new List<GameObject>();

    [Header("Node Prefab")]
    public GameObject nodePrefab;

    [Header("Node Grid Calcs")]
    public Transform gridOrigin;
    public Vector2 gridSize;
    public float nodeSpacing;
    private Vector2 nodePlacement;

    void Update() {
        if (Input.GetKeyDown(KeyCode.O)) {
            createGrid();
        }
    }
    
    // method that creates the node grid for the monster
    public void createGrid() {
        // set up grid
        nodePlacement = new Vector2(gridOrigin.transform.position.x-(gridSize.x/2), gridOrigin.transform.position.z+(gridSize.y/2));

        // instantiate nodes
        for (int i = 0; i < gridSize.x; i++) {
            for (int j = 0; j < gridSize.y; j++) {
                if (checkSpacing()) {
                    GameObject foo = Instantiate(nodePrefab, new Vector3(nodePlacement.x, 2f, nodePlacement.y), nodePrefab.transform.rotation);
                }
                nodePlacement.x += nodeSpacing * 1.25f;
            }
            nodePlacement.x = (gridOrigin.transform.position.x-(gridSize.x/2));
            nodePlacement.y -= nodeSpacing * 1.25f;
        }
    }

    // method that checks the spacing before placing a node
    public bool checkSpacing() {
        Collider[] hitColliders = Physics.OverlapSphere(nodePlacement, nodeSpacing);

        foreach(Collider hit in hitColliders) {
            if (hit.gameObject.tag != "Player" && hit.gameObject.tag != "ground") {
                return false;
            }
        }
        return true;
    }
}
