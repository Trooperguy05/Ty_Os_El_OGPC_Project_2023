using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSuspicion : MonoBehaviour
{
    public int suspicion = 0;

    public GameObject[] mines;
    public List<string> tagList = new List<string>();
    public Vector3 mineGridOrigin;
    public Vector3 mineGridSize;
    public LayerMask groundLayer;

    // when suspicion is high enough, spawn a mine
    void Update() {
        if (suspicion >= 100) {
            suspicion -= 100;
            spawnMine();
        }
    }

    // method that spawns a random mine
    public void spawnMine() {
        // random mine
        int ran = Random.Range(0, mines.Length);

        // random position
        Vector3 ranPos = new Vector3(0, 0, 0);
        do {
            ranPos = mineLocation();
        } while (ranPos == new Vector3(0, 0, 0));

        // spawn the mine
        Instantiate(mines[ran], ranPos, Quaternion.identity);
    }

    // method that checks for a valid mine location
    private Vector3 mineLocation() {
        // random location
        float ranX = Random.Range(-mineGridSize.x, mineGridSize.x);
        float ranZ = Random.Range(-mineGridSize.z, mineGridSize.z);
        
        // check if ground is underneath
        bool isGround = Physics.Raycast(new Vector3(ranX, 2f, ranZ), Vector3.down, 10f, groundLayer);
        if (!isGround) return new Vector3(0, 0, 0);

        // check for space
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(ranX, 2f, ranZ), 5f);

        // check space
        foreach(Collider hit in hitColliders) {
            // no space
            if (tagList.Contains(hit.gameObject.tag)) {
                return new Vector3(0, 0, 0);
            }
        }

        return new Vector3(ranX, 2f, ranZ);
    }
}
