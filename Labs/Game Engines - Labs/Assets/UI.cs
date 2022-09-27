using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public TMP_Text pointDisplay;
    public TMP_Text killDisplay;
    public TMP_Text healthDisplay;

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
        killDisplay.text = "Kills = " + GameManager.GetInstance().Kills;
        healthDisplay.text = "Health = " + GameManager.GetInstance().pcRef.health.CurrentHealth;
    }
}
