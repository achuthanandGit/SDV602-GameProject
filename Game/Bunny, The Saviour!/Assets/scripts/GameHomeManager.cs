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
        GameDescription.text = GameManager.GameManagerInstance.GameModelInstance.DescriptionScene.Story;
    }

    
    /// <summary>Logouts the user session.</summary>
    public void LogoutUser()
    {
        string username = GameManager.GameManagerInstance.Username;
        GameModel.UserLoginDetails[username].UserStatus = "inactive";
        SceneManager.LoadScene("LoginScene");
    }

    
    /// <summary>Starts the new game.</summary>
    public void StartNewGame()
    {
        GameManager.GameManagerInstance.GameMode = "new";
        SceneManager.LoadScene("DialogScene");
    }

    
    /// <summary>Joins the random game.</summary>
    public void JoinRandomGame()
    {
        GameManager.GameManagerInstance.GameMode = "random";
        SceneManager.LoadScene("DialogScene");
    }

}
