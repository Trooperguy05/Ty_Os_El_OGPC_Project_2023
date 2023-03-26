using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            GameObject.Find("Main Camera").GetComponent<MouseLook>().unlockMouse();
            GameObject.Find("Canvas").GetComponent<TimeLeaderboard>().saveData();
            StartCoroutine(GameObject.Find("Scene Loader").GetComponent<SceneLoader>().loadScene(3));
        }
    }
}
