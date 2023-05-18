using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public SceneLoader sL;
    
    // method that loads the main menu
    public void loadMainMenu() {
        StartCoroutine(sL.loadScene(0));
        SceneLoader.changeScene = true;
    }

    // method that loads the credits scene
    public void loadCredits() {
        StartCoroutine(sL.loadScene(4));
        SceneLoader.changeScene = true;
    }
}
