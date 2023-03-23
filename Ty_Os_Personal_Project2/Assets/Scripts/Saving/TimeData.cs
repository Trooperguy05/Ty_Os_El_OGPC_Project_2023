using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeData
{
    public float[] times = new float[5];

    public TimeData(TimeLeaderboard tL) {
        for (int i = 0; i < tL.savedTimes.Length; i++) {
            if (tL.time < tL.savedTimes[i]) {
                tL.savedTimes[i] = tL.time;
                break;
            }
        }

        for (int i = 0; i < times.Length; i++) {
            times[i] = tL.savedTimes[i];
        }
    }
}
