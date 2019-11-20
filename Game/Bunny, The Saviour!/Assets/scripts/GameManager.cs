using Assets.scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts
{
    /// <summary>First class to initiated when the game starts to call static class GameMode</summary>
    public class GameManager : MonoBehaviour
    {

        // to define the current GameManager instance
        private GameManager GameManagerInstance;

        // To define connection error container
        private GameObject ConnectionErrorContainer;

        // To define retry button
        private Button RetryButton;

        /// <summary>
        /// Awakes this instance.
        /// Awake is used to initialize any variables or game state before the game starts.
        /// Called only once during the lifetime of the script instance.
        /// </summary>
        private void Awake()
        {
            ConnectionErrorContainer = GameObject.Find("ConnectionErrorContainer");
            if (GameObject.Find("RetryButton") != null)
            {
                RetryButton = GameObject.Find("RetryButton").GetComponent<Button>();
                RetryButton.onClick.AddListener(CheckAndInitiateGame);
            }
            if(ConnectionErrorContainer != null)
                ConnectionErrorContainer.SetActive(false);
            CheckAndInitiateGame();
        }

        /// <summary>
        /// Checks the and initiate game
        /// </summary>
        private void CheckAndInitiateGame()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("Error. Check internet connection!");
                if (ConnectionErrorContainer != null)
                    ConnectionErrorContainer.SetActive(true);
            }
            else
            {
                if (ConnectionErrorContainer != null)
                    ConnectionErrorContainer.SetActive(false);
                Debug.Log("Successfull internet connection!");
                if (GameManagerInstance is null)
                {
                    Debug.Log("Initiating the GameManager");
                    GameManagerInstance = this;
                    // Creating local database if not exists
                    // setting all the game realted data for the first time
                    GameModel.GetAndSetGameData();
                }
                else
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
}