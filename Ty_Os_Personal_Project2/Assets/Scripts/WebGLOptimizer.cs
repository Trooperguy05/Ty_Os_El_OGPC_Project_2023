using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLOptimizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            Application.targetFrameRate = -1;
        }
        else {
            Application.targetFrameRate = 60;
        }
    }

}
