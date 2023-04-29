using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerDead : MonoBehaviour
{
    public bool isDead;
    public Image fade;
    private PlayerInteract pI;
    private bool fading = false;
    private SceneLoader sL;
    private PlayerSFX pSFX;
    private FMOD.Studio.Bus mBus;

    void Start() {
        pI = GetComponent<PlayerInteract>();
        sL = GameObject.Find("Scene Loader").GetComponent<SceneLoader>();
        pSFX = GetComponent<PlayerSFX>();
        mBus = FMODUnity.RuntimeManager.GetBus("bus:/");
    }

    // method that fails the player
    private IEnumerator playerFail() {
        // set dead to true
        isDead = true;
        pI.canInteract = false;
        // death sound
        mBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        pSFX.ouchy();
        // stop player from moving and free the mouse
        GetComponent<Rigidbody>().freezeRotation = false;
        GameObject.Find("Main Camera").GetComponent<MouseLook>().unlockMouse();
        // fade in red screen
        StartCoroutine(fadeIn());
        // wait then move to fail screen
        while (fading) yield return null;
        yield return new WaitForSeconds(1f);
        StartCoroutine(sL.loadScene(2));
    }
    public void playerFail_wrapper() { StartCoroutine(playerFail()); }

    // method that fades out
    private IEnumerator fadeIn() {
        fading = true;
        float duration = 1.5f;
        float currentTime = 0f;
        while (currentTime < duration) {
            float alphaValue = Mathf.Lerp(0f, 1f, currentTime / duration);
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alphaValue);
            currentTime += Time.deltaTime;
            yield return null;
        }
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 255);
        fading = false;
    }
}
