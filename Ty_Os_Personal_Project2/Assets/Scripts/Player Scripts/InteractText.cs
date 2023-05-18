using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    // updates the interact text to match the string
    public void updateText(string n){
        text.text = n;
    }
}
