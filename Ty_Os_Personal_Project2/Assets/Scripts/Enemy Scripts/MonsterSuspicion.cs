using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSuspicion : MonoBehaviour
{
    public int suspicion = 0;

    public GameObject[] mines;
    public GameObject[] minePlacements;

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
        

        // spawn the mine
        Instantiate(mines[ran]);
    }
}
