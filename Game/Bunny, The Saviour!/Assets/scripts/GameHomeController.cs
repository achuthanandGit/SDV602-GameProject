using Assets.scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHomeController : MonoBehaviour
{
 
    public Text GameDescription;
    // This method is called when the script is activated. Here we can include the code to update the game objects when the scene is loading.
    void Start()
    {
        LoadGameDescription();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is used to update the game description when the GameHome Scene is loaded
    private void LoadGameDescription()
    {
        GameDescription.text = StoryDescriptionTexts.HomeDescriptionText;
    }

    /**
     * this method will load the dialogue scenes between Jack and Tinku
     */
    public void LoadJackTinkuDialogScene()
    {
        SceneManager.LoadScene("JackTinkuDialog");
    }
}
