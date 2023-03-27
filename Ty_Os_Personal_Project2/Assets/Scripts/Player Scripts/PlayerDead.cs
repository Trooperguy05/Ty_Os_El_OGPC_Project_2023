using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDead : MonoBehaviour
{
    public bool isDead;
    private SceneLoader sL;

    void Start() {
        sL = GameObject.Find("Scene Loader").GetComponent<SceneLoader>();
    }

    // method that fails the player
    private IEnumerator playerFail() {
        isDead = true;
        GetComponent<Rigidbody>().freezeRotation = false;
        GameObject.Find("Main Camera").GetComponent<MouseLook>().unlockMouse();

        yield return new WaitForSeconds(2f);
        StartCoroutine(sL.loadScene(2));
    }
    public void playerFail_wrapper() { StartCoroutine(playerFail()); }
}
