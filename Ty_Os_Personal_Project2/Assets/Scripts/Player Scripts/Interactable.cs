using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // interact popup text
    public string promptMessage;
    
    // interact method
    public void BaseInteract() {
        Interact();
    }
    protected virtual void Interact() { }
}
