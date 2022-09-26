using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public TMP_Text pointDisplay;

    private static UI instance;
    public static UI GetInstance() {
        if (instance == null) {
            Debug.Log("UI instance is null");
        }
        return instance;
    }

    private void Awake() 
    {
        instance = this;
    }

    public void UpdateUI() 
    {
        pointDisplay.text = "Points = " + GameManager.GetInstance().Score;
    }
}
