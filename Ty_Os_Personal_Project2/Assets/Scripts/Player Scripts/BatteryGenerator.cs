using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryGenerator : MonoBehaviour
{
    [Header("Setup Vars")]
    public bool spawnedBattery = false;
    public GameObject battery;
    public GameObject batteryParticles;

    [Header("Respawn Vars")]
    public GameObject[] batteryPedestals;
    public float respawnTimer;
    [HideInInspector] public float timer = 0f;
    private GameObject previousPedestal = null;

    [Header("Checking Vars")]
    public float checkTimer;
    private float cTimer = 0f;
    private GameObject inGameBattery;

    // battery spawn routine
    void Update() {
        // check if there is a battery on the map
        cTimer += Time.deltaTime;
        if (cTimer >= checkTimer) {
            inGameBattery = GameObject.Find("Battery");
            cTimer = 0f;
        }

        // spawn the battery if there is no other batteries on the map
        if (inGameBattery == null) timer += Time.deltaTime;

        if (timer >= respawnTimer) {
            spawnedBattery = true;
            timer = 0f;
            spawnBattery();
        }
    }

    // method that spawns a battery a random location
    private void spawnBattery() {
        GameObject foo = null;
        do {
            foo = batteryPedestals[Random.Range(0, batteryPedestals.Length)];
        } while (foo == previousPedestal);

        GameObject boo = Instantiate(battery, foo.transform.GetChild(0).position, Quaternion.identity);
        boo.name = "Battery";
        Instantiate(batteryParticles, foo.transform.GetChild(0).position, Quaternion.identity);
        inGameBattery = boo;
    }

    // method that spawns a battery at a certain location
    // used for forcing a battery to spawn at the cost of suspicion
    public void spawnBattery(GameObject location) {
        // destroy other battery(s) on the map
        GameObject batt = GameObject.Find("Battery");
        if (batt != null) Destroy(batt);
        // instantiate the battery
        GameObject boo = Instantiate(battery, location.transform.GetChild(0).position, Quaternion.identity);
        boo.name = "Battery";
        Instantiate(batteryParticles, location.transform.GetChild(0).position, Quaternion.identity);
    }
}
