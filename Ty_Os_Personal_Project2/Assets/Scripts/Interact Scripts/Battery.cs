using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Interactable
{
    protected override void Interact() {
        Debug.Log("Interacted with " + gameObject.name);

        // place battery in the hand of the player
        Transform grip = GameObject.Find("Object Grip").GetComponent<Transform>();
        gameObject.transform.parent = grip;
        gameObject.transform.position = grip.position;
    }
}