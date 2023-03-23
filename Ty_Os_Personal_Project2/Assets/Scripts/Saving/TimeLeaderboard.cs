using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeLeaderboard : MonoBehaviour
{
    public float time = 0f;
    public TextMeshProUGUI timeText;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeText.text = updateTimeText();
    }

    // method that updates the time text while formatting it
    public string updateTimeText() {
        int minutes = (int) time / 60;
        int seconds = (int) time;

        return minutes + ":" + seconds; 
    }

    // method that saves the current score to the highscore leaderboard, if faster
    public void saveData() {
        TimeData data = SaveSystem.LoadTimeLeaderboard();
        float[] timeboard = new float[5];
        // get previous high scores
        if (data != null) {
            for (int i = 0; i < data.times.Length; i++) {
                timeboard[i] = data.times[i];
            }
        }
        // compare highscores to current score
        for (int i = 0; i < timeboard.Length; i++) {
            if (time < timeboard[i] || timeboard[i] == 0) {
                if (i != timeboard.Length-1) {
                    for (int j = i; j < timeboard.Length-1; j+=2) {
                        timeboard[j+1] = timeboard[j];
                    }
                }
                //if (i != timeboard.Length-1) timeboard[i+1] = timeboard[i];
                timeboard[i] = time;
                break;
            }
        }
        // save the highscores
        SaveSystem.SaveTime(timeboard);
    }
}
