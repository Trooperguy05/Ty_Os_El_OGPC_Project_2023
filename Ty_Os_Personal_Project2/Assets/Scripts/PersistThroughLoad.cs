using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistThroughLoad : MonoBehaviour
{
    // bools to better help the object persist through load
    public bool initialized = false;
    public bool isCanvas = false;

    // adding/removing the update camera method to the sceneChanged event
    void OnEnable() { 
        SceneLoader.sceneChanged += updateCamera;
        SceneLoader.sceneChanged += recheckUnique; 
    }
    void OnDisable() { 
        SceneLoader.sceneChanged -= updateCamera; 
        SceneLoader.sceneChanged -= recheckUnique;
    }

    // only allow one of this object to exist
    void Start()
    {
        int numObj = 0;
        foreach (GameObject gO in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
            if (gO.gameObject.name == gameObject.name) {
                numObj++;
                if (numObj > 1) {
                    Destroy(gO.gameObject);
                    numObj--;
                }
            }
        }
        if (!initialized) {
            initialized = true;
            DontDestroyOnLoad(gameObject);
        }
    }

    // method for canvas objects persisting through load to find the main camera for their render mode \\
    public void updateCamera() {
        if (isCanvas) {
            GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();          
        }
    }

    // method that rechecks that only one of this object exists \\
    public void recheckUnique() {
        int numObj = 0;
        foreach (GameObject gO in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
            if (gO.gameObject.name == gameObject.name) {
                numObj++;
                if (numObj > 1) {
                    Destroy(gO.gameObject);
                    numObj--;
                }
            }
        }
    }
}
