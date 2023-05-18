using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // calculation variables
    public float sensX;
    public float sensY;
    public Transform orientation;
    private float xRotation;
    private float yRotation;

    // supporting variables
    public bool isMenusOpen;
    private PlayerDead pD;

    // get object and set default mouse state
    void Start() {
        pD = GameObject.Find("Player").GetComponent<PlayerDead>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // mouse movement
    void Update() {
        if (!isMenusOpen && !pD.isDead) {
            // mouse calculations
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // rotate cam and orientation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    // method that locks and unlocks the mouse
    public void unlockMouse() {
        isMenusOpen = !isMenusOpen;
        if (isMenusOpen) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
