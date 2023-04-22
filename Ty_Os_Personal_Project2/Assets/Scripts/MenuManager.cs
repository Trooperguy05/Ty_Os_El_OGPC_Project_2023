using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Bools")]
    private bool controlsActive;

    [Header("GameObjects")]
    public GameObject controls;

    // method to be put on a btn
    // switches scene to the play scene
    public void playGame() {
        StartCoroutine(GameObject.Find("Scene Loader").GetComponent<SceneLoader>().loadScene(1));
    }

    // method to be put on a btn
    // exits the game
    public void exitGame() {
        Application.Quit();
    }

    // method to be put on a btn
    // switches the scene to the main menu
    public void mainMenu() {
        StartCoroutine(GameObject.Find("Scene Loader").GetComponent<SceneLoader>().loadScene(0));
    }

    // method to be put on a btn
    // Toggles the controls panel
    public void toggleControls() {
        controlsActive = !controlsActive;
        controls.SetActive(controlsActive);
    }
}
