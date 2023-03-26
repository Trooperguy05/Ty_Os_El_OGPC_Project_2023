using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeData
{
    public List<float> times = new List<float>() {0, 0, 0, 0, 0};

    public TimeData(List<float> highscores) {
        //times = new List<float>() {0, 0, 0, 0, 0};
        for (int i = 0; i < highscores.Count; i++) {
            times[i] = highscores[i];
        }
    }
}
