using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryMine : MonoBehaviour
{
    public bool inEarshot = false;
    public float soundSensitivity;
    private PlayerSoundRadius pSR;

    void Start() {
        pSR = GameObject.Find("Player").GetComponent<PlayerSoundRadius>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inEarshot && pSR.soundValue >= soundSensitivity) {
            // destroy battery in hand \\
            // check for battery
            GameObject grip = GameObject.Find("Object Grip");
            if (grip.transform.childCount == 0) return;

            // if there is a battery
            Mine m = transform.parent.GetComponent<Mine>();
            if (grip.transform.GetChild(0) != null) {
                Destroy(grip.transform.GetChild(0).gameObject);
                m.destroyMine();
            }
        }
    }

    // inearshot calcs
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            inEarshot = true;
        }
    }
    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Player") {
            inEarshot = false;
        }
    }
}
