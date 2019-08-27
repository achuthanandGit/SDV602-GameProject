using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHomeController : MonoBehaviour
{
    // To set the Game story 
    public Text GameDescription;

    /**
    * Start method is used to initialize or assign values or actions to required 
    * variable or components before the first frame update
    */
    void Start()
    {
        LoadGameDescription();
    }


    /**
     * LoadGameDescription method is used to update the game description when the GameHome Scene is loaded
     */
    private void LoadGameDescription()
    {
        GameDescription.text = GameManager.GameManagerInstance.GameModelInstance.DescriptionScene.Story;
    }

    /**
     *  LogoutUser is used to logout the user session
     */
    public void LogoutUser()
    {
        string username = GameManager.GameManagerInstance.Username;
        GameModel.UserLoginDetails[username]["status"] = "inactive";
        SceneManager.LoadScene("LoginScene");
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("JackTinkuDialog");
    }

}
