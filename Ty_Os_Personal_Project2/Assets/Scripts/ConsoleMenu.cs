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
    public string consoleText;
    public bool isConsoleActive;
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
            isConsoleActive = !isConsoleActive;
            console.SetActive(isConsoleActive);
            mouseLook.unlockMouse();
        }
        if (Input.GetKeyDown(KeyCode.Return) && isConsoleActive) {
            storeInput();
        }
    }

    // Stores the input of the text box \\
    public void storeInput() {
        consoleText = inputField.GetComponent<TMP_InputField>().text;
        consoleText = consoleText.ToLower();
        checkCommand();
    }

    // Checks if a valid command is entered and executes it \\
    private void checkCommand() {
        // Help the player learn and understand commands if they choose to use them \\
        if (consoleText == "help") {
            Help();
        }
        // Teleport the player to given location \\
        else if (consoleText.StartsWith("tp")) {
            // Get the location specified in the string \\
            string[] tp = consoleText.Split(' ');
            Teleport(tp[1]);
        }
        // Clear the console menu \\
        else if (consoleText == "clear") {
            consoleLog.GetComponent<TextMeshProUGUI>().text = "";
        }
        // Tell the player that they inputed a non-valid command \\
        else {
            consoleLog.GetComponent<TextMeshProUGUI>().text += "\n-- unknown command \"" + consoleText + "\" --";
            consoleLog.GetComponent<TextMeshProUGUI>().text += "\n-- Use \"help\" for help --";
        }
    }

    // Print the help menu into the console \\
    private void Help() {
        consoleLog.GetComponent<TextMeshProUGUI>().text += "\n-- Nothing is here dumb dumb Do gud lul";
    }

    // Teleport the player \\
    private void Teleport(string position) { 
        // Teleport to the start room \\
        if (position == "startroom") {}// Move here \\
        // Teleport to the end room \\
        else if (position == "endroom") {} // Move here \\
        // Teleport to the normal room \\
        else if (position == "normalroom") {} // Move here \\
        // Teleport the location player specified \\
        else {
            // Get the x,y,z coordinates specified \\
            string[] pos = position.Split(',');
            // Turn the x,y,z coordinates into intergers \\
            int x,y,z = 0;
            x = int.Parse(pos[0]);
            y = int.Parse(pos[1]);
            z = int.Parse(pos[2]);
            // Move the player \\
            consoleLog.GetComponent<TextMeshProUGUI>().text += "\nMoving the player to (" + x + "," + y + "," + z + ")";
            playerTransform.position = new Vector3(x, y, z);

        }
    }
}