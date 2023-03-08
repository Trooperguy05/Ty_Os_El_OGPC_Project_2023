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
    public bool consoleIsActive;
    public string consoleText;
    private bool isConsoleChecked;
    private int startingWaveNumber;
    public GameObject consoleLog;
    public GameObject inputField;


    void Start() {
        // Get stuff \\
        
    }
    
    void Update() {
        // Opens and closes the console \\
        
    }

    // Stores the input of the text box \\
    public void storeInput() {
        consoleText = inputField.GetComponent<TMP_InputField>().text;
        checkCommand();
    }

    // Checks if a valid command is entered and executes it \\
    private void checkCommand() {
        if (consoleText == "Help") {
            consoleLog.GetComponent<TextMeshProUGUI>().text += "\n-----------------------------------------------------";
            consoleLog.GetComponent<TextMeshProUGUI>().text += "";
            consoleLog.GetComponent<TextMeshProUGUI>().text += "\n-----------------------------------------------------";
        }
        else {
            consoleLog.GetComponent<TextMeshProUGUI>().text += "\nunknown command \"" + consoleText + "\"";
        }
    }
}