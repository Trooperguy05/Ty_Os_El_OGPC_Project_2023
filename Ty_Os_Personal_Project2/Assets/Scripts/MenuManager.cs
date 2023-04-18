using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // method to be put on a btn
    // switches scene to the play scene
    public void playGame() {
        GameObject.Find("Scene Loader").GetComponent<SceneLoader>().changeScene = true;
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
}
