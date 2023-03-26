using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SunMine : MonoBehaviour
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
            Mine m = transform.parent.GetComponent<Mine>();
            // player loses
            GameObject.Find("Main Camera").GetComponent<MouseLook>().unlockMouse();
            StartCoroutine(GameObject.Find("Scene Loader").GetComponent<SceneLoader>().loadScene(2));
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
