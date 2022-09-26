using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    // SINGLETON
    // ========================================================

    private static GameManager instance;
    public static GameManager GetInstance() 
    {
        if (instance == null) instance = new GameManager();
        return instance;
    }

    // ========================================================
    
    public int Score = 0;

    public void AddPoint() 
    {
        Score += 1;
        UI.GetInstance().UpdateUI();
    }
}
