using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public SceneLoader sL;
    public void switchScene() {
        StartCoroutine(sL.loadScene(0));
    }
}
