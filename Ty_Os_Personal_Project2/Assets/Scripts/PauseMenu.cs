using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public KeyCode pauseButton;
    public GameObject menu;
    public bool menuOpen = false;
    private MouseLook mL;
    private ConsoleMenu cM;

    void Start() {
        mL = Camera.main.GetComponent<MouseLook>();
        cM = GameObject.Find("Console Manager").GetComponent<ConsoleMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseButton) && !cM.isConsoleActive) {
            menuOpen = !menuOpen;
            menu.SetActive(menuOpen);
            mL.unlockMouse();
        }
    }
}
