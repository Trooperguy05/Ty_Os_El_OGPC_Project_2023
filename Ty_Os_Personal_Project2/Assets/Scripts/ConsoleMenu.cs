using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConsoleMenu : MonoBehaviour
{
    // Declaring Varibles \\
    [Header("Scripts")]
    private MouseLook mouseLook;
    private Stamina stamina;
    private MonsterMovementNavmesh monsterAI;
    private PlayerMovementTest playerSpeed;
    private PauseMenu pauseMenu;
    
    [Header("GameObjects")]
    public GameObject console;
    public GameObject consoleLog;
    public GameObject inputField;
    public GameObject player;
    public GameObject battery;
    
    [Header("Transforms")]
    public Transform playerTransform;
    
    [Header("Variables")]
    public string consoleText;
    public bool isConsoleActive;
    private int startingWaveNumber;
    private bool unlimitedStaminaActive;
    private float staminaConsumption;
    private int roomNum;
    
    
    
    void Start() {
        // Get stuff \\
        mouseLook = GameObject.Find("Main Camera").GetComponent<MouseLook>();
        stamina = GameObject.Find("Player").GetComponent<Stamina>();
        staminaConsumption = stamina.staminaConsumeRate;
        monsterAI = GameObject.Find("Monster").GetComponent<MonsterMovementNavmesh>();
        playerSpeed = player.GetComponent<PlayerMovementTest>();
        pauseMenu = GameObject.Find("Canvas").GetComponent<PauseMenu>();
    }
    
    void Update() {
        // Opens and closes the console \\
        if (Input.GetKeyDown(KeyCode.BackQuote) && !pauseMenu.menuOpen) {
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
        // Toggle unlimited stamina command \\
        else if (consoleText == "unlimitedstamina") {
            // Change bool to opposite value \\
            unlimitedStaminaActive = !unlimitedStaminaActive;
            // Activate unlimited stamina \\
            if (unlimitedStaminaActive) {
                stamina.staminaConsumeRate = 0.0f;
            }
            // Deactivate unlimited stamina \\
            else {
                stamina.staminaConsumeRate = staminaConsumption;
            }
        }
        // Clear the console menu \\
        else if (consoleText == "clear") {
            consoleLog.GetComponent<TextMeshProUGUI>().text = "";
        }
        // Turn off monster AI \\
        else if (consoleText == "toggleai") {
            monsterAI.AIOn = !monsterAI.AIOn;
        }

        else if (consoleText.StartsWith("playerspeed")) {
            string[] speed = consoleText.Split(' ');
            PlayerSpeedChange(speed[1]);
        }
        // Give battery object to player \\
        else if (consoleText.StartsWith("givebattery")) {
            Instantiate(battery, player.transform.position, Quaternion.identity);
        }
        // Reset Scene \\ 
        else if (consoleText.StartsWith("resetscene")) {
            SceneManager.LoadScene(0);
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

    private void PlayerSpeedChange(string speed) {
        int walkSpeed = int.Parse(speed);
        playerSpeed.walkSpeed = walkSpeed;
    }
    // Teleport the player \\
    private void Teleport(string position) { 
        // Teleport to the start room \\
        if (position == "startroom") {
            playerTransform.position = new Vector3(-3.19f, 1.0f, -1.87f);
        }
        // teleport to the end room \\ 
        else if (position == "endroom") {
            playerTransform.position = new Vector3(-50.34f, 1.0f, 101.2f);
        } 
        else if (position == "batteryroom1") {
            playerTransform.position = new Vector3(11.39f, 1.0f, 98.7f);
        }
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
