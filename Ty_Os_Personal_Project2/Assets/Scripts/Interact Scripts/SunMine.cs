using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMine : MonoBehaviour
{
    public bool inEarshot = false;
    public float soundSensitivity;
    public GameObject explosion;
    private PlayerSoundRadius pSR;

    void Start() {
        pSR = GameObject.Find("Player").GetComponent<PlayerSoundRadius>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inEarshot && pSR.soundValue >= soundSensitivity) {
            // player loses
            Debug.LogError("Player Loses!!!");
            Destroy(gameObject);
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

    // method that destroys the mine with particles
    public void destroyMine() {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(transform.parent.gameObject);
    }
}
