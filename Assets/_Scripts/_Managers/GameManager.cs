using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameMode { StoryMode, EndlessMode }
    
    [Header("Configuration")]
    public GameMode gameMode;

    void Start()
    {
        
    }

    private void Update()
    {
        Debug_Inputs();
    }

    public void SetGameState(string state, bool stateBool)
    {
        if (stateBool) Time.timeScale = 0f; else Time.timeScale = 1f;
        switch (state)
        {
            case "Pause":
                print("Pause");
                break;
            case "Loose":
                print("Loose");
                break;
        }
    }

    private void Debug_Inputs()
    {        
        if (Input.GetKeyDown("p"))
        {
            SetGameState("Pause", true);
        }
    }

}
