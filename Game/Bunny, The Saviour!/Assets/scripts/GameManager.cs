using Assets.scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>First class to initiated when the game starts to call static class GameMode</summary>
public class GameManager : MonoBehaviour
{
  
    // to define the current GameManager instance
    private GameManager GameManagerInstance;


    /// <summary>
    /// Awakes this instance.
    /// Awake is used to initialize any variables or game state before the game starts.
    /// Called only once during the lifetime of the script instance.
    /// </summary>
    private void Awake()
    {
        if (GameManagerInstance is null)
        {
            Debug.Log("Initiating the GameManager");
            GameManagerInstance = this;
            // Creating local database if not exists
            Debug.Log("Creating Database");
            // setting all the game realted data for the first time
            GameModel.GetAndSetGameData();
        }
        else
            Destroy(gameObject);
    }

    /// <summary>Determines whether [is game running].</summary>
    /// <returns>
    ///   <c>true</c> if [is game running]; otherwise, <c>false</c>.</returns>
    public bool IsGameRunning()
    {
        return true;
    }
}
