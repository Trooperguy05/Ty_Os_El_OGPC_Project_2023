using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public SceneLoader sL;
    
    public void loadMainMenu() {
        StartCoroutine(sL.loadScene(0));
    }

    public void loadCredits() {
        StartCoroutine(sL.loadScene(4));
    }
}
