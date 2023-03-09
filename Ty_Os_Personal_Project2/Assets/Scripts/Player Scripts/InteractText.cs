using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void updateText(string n){
        text.text = n;
    }
}
