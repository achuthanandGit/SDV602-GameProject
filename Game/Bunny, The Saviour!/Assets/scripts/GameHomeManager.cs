using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHomeManager : MonoBehaviour
{
    // To set the Game story 
    public Text GameDescription;

    // MessagePanel to show error/warning/info messages
    public GameObject MessagePanel;

    // MessagePanelText to set the error/warning/info messages in MessagePanel
    public Text MessagePanelText;


    /// <summary>Starts this instance.
    /// Start method is used to initialize or assign values or actions to required variable or components before the first frame update.
    /// </summary>
    void Start()
    {
        LoadGameDescription();
    }


    /// <summary>Loads the game description.</summary>
    private void LoadGameDescription()
    {
        Debug.Log("Putting game description");
        GameDescription.text = GameModel.GameHomeDescription.Story;
    }

    
    /// <summary>Logouts the user session.</summary>
    public void LogoutUser()
    {
        Debug.Log("Logout User");
        string username = GameModel.Username;
        GameModel.LogoutUser(username);
        Debug.Log("Loading Login Scene");
        SceneManager.LoadScene("LoginScene");
    }

    
    /// <summary>Starts the new game.</summary>
    public void StartNewGame()
    {
        Debug.Log("Start New Game");
        // setting the game mode to 'new' for future refernece
        GameModel.GameMode = "new";
        // creating new game
        GameModel.StartNewGame();
        SceneManager.LoadScene("DialogScene");
    }

    
    /// <summary>Joins the random game.</summary>
    public void JoinRandomGame()
    {
        Debug.Log("Join Random Game");
        // setting the game mode to 'random' for future refernece
        GameModel.GameMode = "random";
        // finding random game to join
        if(GameModel.GetRandomGameToJoin())
            SceneManager.LoadScene("DialogScene");
        else
        {
            // showoing info message if fails to find random game
            MessagePanelText.text = "At the moment there is no active game to join. Please create a new game to start.";
            MessagePanel.SetActive(true);
        }
    }

    /// <summary>
    /// Handles the message panel button.
    /// Close the message panel when OK button is clicked
    /// </summary>
    public void HandleMessagePanelButton()
    {
        MessagePanel.SetActive(false);
    }
}
