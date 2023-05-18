using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("KeyCodes")]
    public KeyCode pauseButton;
    [Header("GameObjects")]
    public GameObject menu;
    public GameObject audioMenu;
    [Header("Bools")]
    public bool menuOpen = false;
    [Header("Scripts")]
    private MouseLook mL;
    private ConsoleMenu cM;

    // get objects
    void Start() {
        mL = Camera.main.GetComponent<MouseLook>();
        cM = GameObject.Find("Console Manager").GetComponent<ConsoleMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        // open/close the pause menu
        if (Input.GetKeyDown(pauseButton) && !cM.isConsoleActive && !audioMenu.activeSelf) {
            menuOpen = !menuOpen;
            menu.SetActive(menuOpen);
            mL.unlockMouse();
        }
        // close the audio menu
        else if (Input.GetKeyDown(pauseButton) && !cM.isConsoleActive && audioMenu.activeSelf) {
            menu.SetActive(menuOpen);
            audioMenu.SetActive(false);
        }
    }
}
