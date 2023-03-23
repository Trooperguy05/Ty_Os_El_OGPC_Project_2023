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
            // format the data

            // print the data
            for (int i = 0; i < data.times.Length; i++) {
                board += (i+1) + ") " + data.times[i] + "\n";
            }
        }
        text.text = board;
    }
}
