using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneLoader : MonoBehaviour
{
    // scene change event
    public delegate void SceneChanged();
    public static event SceneChanged sceneChanged;
    // before scene change event
    public delegate void BeforeSceneChanged();
    public static event BeforeSceneChanged beforeSceneChanged;

    // loading screen and scene loader variables \\
    public static bool changeScene = false;
    
    // method that loads the next scene in the build setting \\
    public void nextScene() {
        // stop loading coroutines
        StopAllCoroutines();
        // load the next scene in the build settings
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene > SceneManager.sceneCountInBuildSettings-1) nextScene = 0;
        StartCoroutine(loadScene(nextScene));
    }

    // method that reloads the current scene \\
    public void reloadScene() {
        StopAllCoroutines();
        StartCoroutine(loadScene(SceneManager.GetActiveScene().buildIndex));
    }

    // method that loads the next scene based on the build index given \\
    public IEnumerator loadScene(int index) {
        // invoke the beforescenechanged event
        if (beforeSceneChanged != null) beforeSceneChanged.Invoke();
        
        // load the scene
        AsyncOperation aO = SceneManager.LoadSceneAsync(index);

        // load scene
        while (!aO.isDone) yield return null;

        // invoke the sceneChanged event
        if (sceneChanged != null) sceneChanged.Invoke();

        
        while (!aO.isDone) {
            if (changeScene) {
                aO.allowSceneActivation = true;
            }
            yield return null;
        }
        
    }
}
