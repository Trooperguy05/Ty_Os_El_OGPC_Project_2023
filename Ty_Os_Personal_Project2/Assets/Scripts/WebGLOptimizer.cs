using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLOptimizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // LMAO
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            Application.targetFrameRate = -1;
        }
        // Goofy Ahhh
        else {
            Application.targetFrameRate = 60;
        }
    }

}
