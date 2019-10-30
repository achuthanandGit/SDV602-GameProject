using Assets.scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  
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
            GameModel.GetAndSetGameData();


            //string filePath = Application.persistentDataPath + "/UserDetails.dat";
            //GameModelInstance = new GameModel(Application.persistentDataPath + "/UserDetails.dat");
            //GameModel.RetrieveData();
        }
        else
        {
            //GameModel.SaveData();
            Destroy(gameObject);
        }
    }

    /// <summary>Determines whether [is game running].</summary>
    /// <returns>
    ///   <c>true</c> if [is game running]; otherwise, <c>false</c>.</returns>
    public bool IsGameRunning()
    {
        return true;
    }
}
