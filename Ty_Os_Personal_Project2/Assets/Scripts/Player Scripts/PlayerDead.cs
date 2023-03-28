using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDead : MonoBehaviour
{
    public bool isDead;
    private SceneLoader sL;
    private PlayerSFX pSFX;
    private FMOD.Studio.Bus mBus;

    void Start() {
        sL = GameObject.Find("Scene Loader").GetComponent<SceneLoader>();
        pSFX = GetComponent<PlayerSFX>();
        mBus = FMODUnity.RuntimeManager.GetBus("bus:/");
    }

    // method that fails the player
    private IEnumerator playerFail() {
        isDead = true;
        mBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        pSFX.ouchy();
        GetComponent<Rigidbody>().freezeRotation = false;
        GameObject.Find("Main Camera").GetComponent<MouseLook>().unlockMouse();

        yield return new WaitForSeconds(2f);
        StartCoroutine(sL.loadScene(2));
    }
    public void playerFail_wrapper() { StartCoroutine(playerFail()); }
}
