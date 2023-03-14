using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConsoleMenu : MonoBehaviour
{
    // Declaring Varibles \\
    public GameObject console;
    public Transform playerTransform;
    public bool consoleIsActive;
    public string consoleText;
    private bool isConsoleChecked;
    private int startingWaveNumber;
    public GameObject consoleLog;
    public GameObject inputField;
    private MouseLook mouseLook;


    void Start() {
        // Get stuff \\
        mouseLook = GameObject.Find("Main Camera").GetComponent<MouseLook>();
    }
    
    void Update() {
        // Opens and closes the console \\
        if (Input.GetKeyDown(KeyCode.BackQuote)) {
            isConsoleChecked = !isConsoleChecked;
            console.SetActive(isConsoleChecked);
            mouseLook.unlockMouse();
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            storeInput();
        }
    }

    // Stores the input of the text box \\
    public void storeInput() {
        consoleText = inputField.GetComponent<TMP_InputField>().text;
        consoleText.ToLower();
        checkCommand();
    }

    // Checks if a valid command is entered and executes it \\
    private void checkCommand() {
        // Help the player learn and understand commands if they choose to use them \\
        if (consoleText == "help") {
            consoleLog.GetComponent<TextMeshProUGUI>().text += "\nNothing is here dumb dumb Do gud lul";
        }
        else if (consoleText.StartsWith("tp")) {
            string[] tp = consoleText.Split(' ');
            Teleport(tp[1]);
        }
        // Clear the console menu \\
        else if (consoleText == "clear") {
            consoleLog.GetComponent<TextMeshProUGUI>().text = "";
        }
        else {
            consoleLog.GetComponent<TextMeshProUGUI>().text += "\nunknown command \"" + consoleText + "\"";
        }
    }

    private void Teleport(string position) { 
        if (position == "start room") {}// Move here \\
        else if (position == "end room") {} // Move here \\
        else if (position == "normal room") {} // Move here \\
        else {
            string[] pos = position.Split(',');
            int x,y,z = 0;
            x = int.Parse(pos[0]);
            y = int.Parse(pos[1]);
            z = int.Parse(pos[2]);
            playerTransform.position = new Vector3(x, y, z);

        }
    }
}