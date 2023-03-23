using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeData
{
    public float[] times = new float[5];

    public TimeData(float[] highscores) {
        for (int i = 0; i < highscores.Length; i++) {
            times[i] = highscores[i];
        }
    }
}
