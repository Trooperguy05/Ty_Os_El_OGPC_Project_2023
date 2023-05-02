using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLOptimizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Force WebGL to run at browser recommended frame rate to prevent studdering
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            Application.targetFrameRate = -1;
        }
        // Other playforms should run at 60 fps
        else {
            Application.targetFrameRate = 60;
        }
    }

}
