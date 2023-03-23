using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeLeaderboard : MonoBehaviour
{
    [HideInInspector] public float[] savedTimes;
    public float time = 0f;
    public TextMeshProUGUI timeText;

    void Awake() {
        savedTimes = new float[5];
        loadData();
    }

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

    // methods for saving and loading the times
    public void loadData() {
        TimeData data = SaveSystem.LoadTimeLeaderboard();

        if (data != null) {
            savedTimes = data.times;
        }
    }
    public void saveData() {
        SaveSystem.SaveTime(this);
    }
}
