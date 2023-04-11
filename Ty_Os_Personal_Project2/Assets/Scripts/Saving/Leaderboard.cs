using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        TimeData data = SaveSystem.LoadTimeLeaderboard();

        string board = "";
        if (data != null) {
            // print the data
            for (int i = 0; i < data.times.Count; i++) {
                board += (i+1) + ") " + formatTime(data.times[i]) + "\n";
            }
        }
        text.text += "\n" + board;
    }

    // method that formats the time given into a usable string
    public string formatTime(float time) {
        int minutes = (int) time / 60;
        int seconds = (int) time - (minutes * 60);
        int miliseconds = (int) ((time - ((int) time)) * 100f);

        return minutes + ":" + seconds + ":" + miliseconds; 
    }
}
