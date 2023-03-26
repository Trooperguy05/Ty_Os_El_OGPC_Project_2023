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
        int seconds = (int) time - minutes;
        int miliseconds = (int) ((time - ((int) time)) * 100f);

        return minutes + ":" + seconds + ":" + miliseconds; 
    }

    // method that saves the current score to the highscore leaderboard, if faster
    public void saveData() {
        // funky set up
        TimeData data = SaveSystem.LoadTimeLeaderboard();
        List<float> timeboard = new List<float>();

        // get previous high scores
        if (data != null) for (int i = 0; i < data.times.Count; i++) timeboard.Add(data.times[i]);
        else for (int i = 0; i < 5; i++) timeboard.Add(0);

        // compare highscores to current score
        for (int i = 0; i < timeboard.Count; i++) {
            if (time < timeboard[i] || timeboard[i] == 0) {
                timeboard.Insert(i, time);
                timeboard.RemoveAt(5);
                break;
            }
        }

        // save the highscores
        SaveSystem.SaveTime(timeboard);
    }
}
